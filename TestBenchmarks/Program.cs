/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// Portions copyright (C) 2018 Matt R. Cole 
/// ------------------------------------------------------

using System;
using SwarmOps;
using SwarmOps.Optimizers;
using SwarmOps.Problems;
using Console = Colorful.Console;

namespace TestBenchmarks
{
    using System.Collections.Generic;
    using System.Data.SqlTypes;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices.ComTypes;
    using BenchmarkDotNet.Attributes;
    using BenchmarkDotNet.Attributes.Columns;
    using BenchmarkDotNet.Attributes.Exporters;
    using BenchmarkDotNet.Attributes.Jobs;
    using BenchmarkDotNet.Configs;
    using BenchmarkDotNet.Jobs;
    using BenchmarkDotNet.Running;
    using BenchmarkDotNet.Validators;
    using Colorful;
    using NodaTime;

    /// <summary>
    /// Test an optimizer on various benchmark problems.
    /// </summary>
    [ClrJob(isBaseline: true), CoreJob]
    [RankColumn, CsvMeasurementsExporter()]
    class Program
    {
        // Create optimizer object.
        static readonly Optimizer Optimizer = new DE();

        // Control parameters for optimizer.
        static readonly double[] Parameters = Optimizer.DefaultParameters;

        // Optimization settings.
        static readonly int NumRuns = 50;
        static readonly int Dim = 5;
        static readonly int DimFactor = 2000;
        static readonly int NumIterations = DimFactor* Dim;

        // Mangle search-space.
        static readonly bool UseMangler = false;
        static readonly double Spillover = 0.05;       // E.g. 0.05
        static readonly double Displacement = 0.1;     // E.g. 0.1
        static readonly double Diffusion = 0.01;       // E.g. 0.01
        static readonly double FitnessNoise = 0.01;    // E.g. 0.01

       
        /// <summary>
        /// Optimize the given problem and output result-statistics.
        /// </summary>
        static void Optimize(Problem problem)
        {
            if (UseMangler)
            {
                // Wrap problem-object in search-space mangler.
                problem = new Mangler(problem, Diffusion, Displacement, Spillover, FitnessNoise);
            }

            // Create a fitness trace for tracing the progress of optimization re. mean.
            int NumMeanIntervals = 3000;
            FitnessTrace fitnessTraceMean = new FitnessTraceMean(NumIterations, NumMeanIntervals);

            // Create a fitness trace for tracing the progress of optimization re. quartiles.
            // Note that fitnessTraceMean is chained to this object by passing it to the
            // constructor, this causes both fitness traces to be used.
            int NumQuartileIntervals = 10;
            FitnessTrace fitnessTraceQuartiles = new FitnessTraceQuartiles(NumRuns, NumIterations, NumQuartileIntervals, fitnessTraceMean);

            // Create a feasibility trace for tracing the progress of optimization re. feasibility.
            FeasibleTrace feasibleTrace = new FeasibleTrace(NumIterations, NumMeanIntervals, fitnessTraceQuartiles);

            // Assign the problem etc. to the optimizer.
            Optimizer.Problem = problem;
            Optimizer.FitnessTrace = feasibleTrace;

            // Wrap the optimizer in a logger of result-statistics.
            bool StatisticsOnlyFeasible = true;
            Statistics Statistics = new Statistics(Optimizer, StatisticsOnlyFeasible);

            // Wrap it again in a repeater.
            Repeat Repeat = new RepeatSum(Statistics, NumRuns);

            // Perform the optimization runs.
            double fitness = Repeat.Fitness(Parameters);

            // Compute result-statistics.
            Statistics.Compute();

            // Output result-statistics
            string msg = "{0} = {1} - {2} = {3} = {4} = {5} = {6} = {7} = {8} \r\n";

            Console.WriteFormatted(msg, Color.OrangeRed, Color.Green, 
                problem.Name,
                Tools.FormatNumber(Statistics.FitnessMean),
                Tools.FormatNumber(Statistics.FitnessStdDev),
                Tools.FormatNumber(Statistics.FitnessQuartiles.Min),
                Tools.FormatNumber(Statistics.FitnessQuartiles.Q1),
                Tools.FormatNumber(Statistics.FitnessQuartiles.Median),
                Tools.FormatNumber(Statistics.FitnessQuartiles.Q3),
                Tools.FormatNumber(Statistics.FitnessQuartiles.Max),
                Tools.FormatPercent(Statistics.FeasibleFraction));

            // Output fitness and feasible traces.
            fitnessTraceMean.WriteToFile(Optimizer.Name + "-FitnessTraceMean-" + problem.Name + ".txt");
            fitnessTraceQuartiles.WriteToFile(Optimizer.Name + "-FitnessTraceQuartiles-" + problem.Name + ".txt");
            feasibleTrace.WriteToFile(Optimizer.Name + "-FeasibleTrace-" + problem.Name + ".txt");
        }

        static void Main(string[] args)
        {
            Int32 a = -123456789, b = 234567890;
            UInt32 aU = (UInt32)a, bU = (UInt32)b;

            Int32 c = a + b;
            UInt32 cU = aU + bU;

            // Initialize PRNG.
            Globals.Random = new RandomOps.MersenneTwister();

            // Output optimization settings.
            Console.WriteLine("BenchmarkProblem-tests.", Color.Yellow);
            Console.WriteLine("Optimizer: {0}", Optimizer.Name, Color.Yellow);
            Console.WriteLine("Using following parameters:", Color.Yellow);
            Tools.PrintParameters(Optimizer, Parameters);
            Console.WriteLine("Number of runs per problem: {0}", NumRuns, Color.Yellow);
            Console.WriteLine("Dimensionality: {0}", Dim, Color.Yellow);
            Console.WriteLine("Dim-factor: {0}", DimFactor, Color.Yellow);
            if (UseMangler)
            {
                Console.WriteLine("Mangle search-space:");
                Console.WriteLine("\tSpillover:     {0}", Spillover, Color.Red);
                Console.WriteLine("\tDisplacement:  {0}", Displacement, Color.Red);
                Console.WriteLine("\tDiffusion:     {0}", Diffusion, Color.Red);
                Console.WriteLine("\tFitnessNoise:  {0}", FitnessNoise, Color.Red);
            }
            else
            {
                Console.WriteLine("Mangle search-space: No", Color.Yellow);
            }
            Console.WriteLine();
            Console.WriteLine("Problem = Mean = Std.Dev. = Min = Q1 = Median = Q3 = Max = Feasible", Color.LightBlue);
            Console.WriteLine("");

            // Starting-time.
            ZonedDateTime t1 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();

#if false
            //Optimize(new Ackley(Dim, NumIterations));
            //Optimize(new Rastrigin(Dim, NumIterations));
            //Optimize(new Griewank(Dim, NumIterations));
            //Optimize(new Rosenbrock(Dim, NumIterations));
            //Optimize(new Schwefel12(Dim, NumIterations));
            Optimize(new Sphere(Dim, NumIterations));
            //Optimize(new Step(Dim, NumIterations));
#else
            List<Formatter> names = new List<Formatter>();
            foreach (var problem in Benchmarks.IDs
                .Select(benchmarkID => benchmarkID.CreateInstance(Dim, NumIterations)))
            {
                names.Add(new Formatter(problem.Name, Color.LightGreen));
            }

            // Optimize all benchmark problems.
            foreach (var problem in Benchmarks.IDs
                .Select(benchmarkID => benchmarkID.CreateInstance(Dim, NumIterations)))
            {
                // Optimize the problem.
                Optimize(problem);
            }
#endif
            // End-time.
            ZonedDateTime t2 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();
            Duration diff = t2.ToInstant() - t1.ToInstant();

            // Output time-usage.
            Console.WriteLine();
            Console.WriteLine("Time usage: {0}", diff, Color.Yellow);
            Console.WriteLine("Press any key to exit", Color.Yellow);
            Console.ReadKey();
        }
    }
}
