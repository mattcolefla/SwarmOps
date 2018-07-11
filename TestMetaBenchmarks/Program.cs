/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// Portions copyright (C) 2018 Matt R. Cole www.evolvedaisolutions.com
/// ------------------------------------------------------

using System;

using SwarmOps;
using SwarmOps.Problems;
using SwarmOps.Optimizers;
using Console = Colorful.Console;
using System.Drawing;

namespace TestMetaBenchmarks
{
    using NodaTime;
    using NodaTime.Extensions;

    /// <summary>
    /// Test meta-optimization, that is, tuning of control parameters
    /// for an optimizer by applying an additional layer of optimization.
    /// See TestParallelMetaBenchmarks for the parallel version of this.
    /// </summary>
    class Program
    {
        // Settings for the optimization layer.
        static readonly int NumRuns = 50;
        static readonly int Dim = 5;
        static readonly int DimFactor = 2000;
        static readonly int NumIterations = DimFactor * Dim;

        // Mangle search-space.
        static readonly bool UseMangler = true;
        static readonly double Spillover = 0.05;       // E.g. 0.05
        static readonly double Displacement = 0.1;     // E.g. 0.1
        static readonly double Diffusion = 0.01;       // E.g. 0.01
        static readonly double FitnessNoise = 0.01;    // E.g. 0.01

        // Wrap problem-object in search-space mangler.
        static Problem Mangle(Problem problem)
        {
            return (UseMangler) ? (new Mangler(problem, Diffusion, Displacement, Spillover, FitnessNoise)) : (problem);
        }

        // The optimizer whose control parameters are to be tuned.
        static Optimizer Optimizer = new MOL();
        //static Optimizer Optimizer = new DESuite(DECrossover.Variant.Rand1Bin, DESuite.DitherVariant.None);

        // Problems to optimize. That is, the optimizer is having its control
        // parameters tuned to work well on these problems. The numbers are weights
        // that signify mutual importance of the problems in tuning. Higher weight
        // means more importance.
        static WeightedProblem[] WeightedProblems =
            new WeightedProblem[]
            {
                new WeightedProblem(1.0, Mangle(new Ackley(Dim, NumIterations))),
                new WeightedProblem(1.0, Mangle(new Griewank(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new Penalized1(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new Penalized2(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new QuarticNoise(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new Rastrigin(Dim, NumIterations))),
                new WeightedProblem(1.0, Mangle(new Rosenbrock(Dim, NumIterations))),
                new WeightedProblem(1.0, Mangle(new Schwefel12(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new Schwefel221(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new Schwefel222(Dim, NumIterations))),
                //new WeightedProblem(1.0, Mangle(new Sphere(Dim, NumIterations))),
                new WeightedProblem(1.0, Mangle(new Step(Dim, NumIterations))),
            };

        // Settings for the meta-optimization layer.
        static readonly int MetaNumRuns = 5;
        static readonly int MetaDim = Optimizer.Dimensionality;
        static readonly int MetaDimFactor = 20;
        static readonly int MetaNumIterations = MetaDimFactor * MetaDim;

        // The meta-fitness consists of computing optimization performance
        // for the problems listed above over several optimization runs and
        // sum the results, so we wrap the Optimizer-object in a
        // MetaFitness-object which takes care of this.
        static MetaFitness MetaFitness = new MetaFitness(Optimizer, WeightedProblems, NumRuns, MetaNumIterations);

        // Print meta-optimization progress.
        static FitnessPrint MetaFitnessPrint = new FitnessPrint(MetaFitness);

        // Log all candidate solutions.
        static int LogCapacity = 20;
        static bool LogOnlyFeasible = false;
        static LogSolutions LogSolutions = new LogSolutions(MetaFitnessPrint, LogCapacity, LogOnlyFeasible);

        // The meta-optimizer.
        static Optimizer MetaOptimizer = new LUS(LogSolutions);

        // Control parameters to use for the meta-optimizer.
        static double[] MetaParameters = MetaOptimizer.DefaultParameters;

        // If using DE as meta-optimizer, use these control parameters.
        //static double[] MetaParameters = DE.Parameters.ForMetaOptimization;

        // Wrap the meta-optimizer in a Statistics object for logging results.
        static readonly bool StatisticsOnlyFeasible = true;
        static Statistics Statistics = new Statistics(MetaOptimizer, StatisticsOnlyFeasible);

        // Repeat a number of meta-optimization runs.
        static Repeat MetaRepeat = new RepeatMin(Statistics, MetaNumRuns);

        static void Main(string[] args)
        {
            // Initialize the PRNG.
            Globals.Random = new RandomOps.MersenneTwister();

            // Create a fitness trace for tracing the progress of meta-optimization.
            int MaxMeanIntervals = 3000;
            FitnessTrace fitnessTrace = new FitnessTraceMean(MetaNumIterations, MaxMeanIntervals);
            FeasibleTrace feasibleTrace = new FeasibleTrace(MetaNumIterations, MaxMeanIntervals, fitnessTrace);

            // Assign the fitness trace to the meta-optimizer.
            MetaOptimizer.FitnessTrace = feasibleTrace;

            // Output settings.
            Console.WriteLine("Meta-Optimization of benchmark problems.", Color.Yellow);
            Console.WriteLine();
            Console.WriteLine("Meta-method: {0}", MetaOptimizer.Name, Color.Yellow);
            Console.WriteLine("Using following parameters:", Color.Yellow);
            Tools.PrintParameters(MetaOptimizer, MetaParameters);
            Console.WriteLine("Number of meta-runs: {0}", MetaNumRuns, Color.Yellow);
            Console.WriteLine("Number of meta-iterations: {0}", MetaNumIterations, Color.Yellow);
            Console.WriteLine();
            Console.WriteLine("Method to be meta-optimized: {0}", Optimizer.Name, Color.Yellow);
            Console.WriteLine("Number of benchmark problems: {0}", WeightedProblems.Length, Color.Yellow);

            foreach (var t in WeightedProblems)
            {
                Problem problem = t.Problem;
                double weight = t.Weight;

                Console.WriteLine("\t({0})\t{1}", weight, problem.Name, Color.Yellow);
            }

            Console.WriteLine("Dimensionality for each benchmark problem: {0}", Dim, Color.Yellow);
            Console.WriteLine("Number of runs per benchmark problem: {0}", NumRuns, Color.Yellow);
            Console.WriteLine("Number of iterations per run: {0}", NumIterations, Color.Yellow);
            if (UseMangler)
            {
                Console.WriteLine("Mangle search-space:");
                Console.WriteLine("\tSpillover:     {0}", Spillover, Color.Yellow);
                Console.WriteLine("\tDisplacement:  {0}", Displacement, Color.Yellow);
                Console.WriteLine("\tDiffusion:     {0}", Diffusion, Color.Yellow);
                Console.WriteLine("\tFitnessNoise:  {0}", FitnessNoise, Color.Yellow);
            }
            else
            {
                Console.WriteLine("Mangle search-space: No", Color.Red);
            }
            Console.WriteLine();

            Console.WriteLine("0/1 Boolean whether optimizer's control parameters are feasible.", Color.Yellow);
            Console.WriteLine("*** Indicates meta-fitness/feasibility is an improvement.", Color.Yellow);

            // Starting-time.
            ZonedDateTime t1 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();

            // Perform the meta-optimization runs.
            double fitness = MetaRepeat.Fitness(MetaParameters);

            ZonedDateTime t2 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();
            Duration diff = t2.ToInstant() - t1.ToInstant();

            // Compute result-statistics.
            Statistics.Compute();

            // Retrieve best-found control parameters for the optimizer.
            double[] bestParameters = Statistics.BestResult.Parameters;

            // Output results and statistics.
            Console.WriteLine();
            Console.WriteLine("Best found parameters for {0} optimizer:", Optimizer.Name, Color.Green);
            Tools.PrintParameters(Optimizer, bestParameters);
            Console.WriteLine("Parameters written in array notation:", Color.Green);
            Console.WriteLine("\t{0}", Tools.ArrayToString(bestParameters, 4), Color.Green);
            Console.WriteLine("Best parameters have meta-fitness: {0}", Tools.FormatNumber(Statistics.FitnessMin), Color.Green);
            Console.WriteLine("Worst meta-fitness: {0}", Tools.FormatNumber(Statistics.FitnessMax), Color.Green);
            Console.WriteLine("Mean meta-fitness: {0}", Tools.FormatNumber(Statistics.FitnessMean), Color.Green);
            Console.WriteLine("StdDev for meta-fitness: {0}", Tools.FormatNumber(Statistics.FitnessStdDev), Color.Green);

            // Output best found parameters.
            Console.WriteLine();
            Console.WriteLine("Best {0} found parameters:", LogSolutions.Capacity, Color.Green);
            foreach (Solution candidateSolution in LogSolutions.Log)
            {
                Console.WriteLine("\t{0}\t{1}\t{2}",
                    Tools.ArrayToStringRaw(candidateSolution.Parameters, 4),
                    Tools.FormatNumber(candidateSolution.Fitness),
                    (candidateSolution.Feasible) ? (1) : (0), Color.Green);
            }

            // Output time-usage.
            Console.WriteLine();
            Console.WriteLine("Time usage: {0}", t2 - t1, Color.Yellow);

            // Output fitness trace.
            string traceFilename
                = MetaOptimizer.Name + "-" + Optimizer.Name
                + "-" + WeightedProblems.Length + "Bnch" + "-" + DimFactor + "xDim.txt";
            fitnessTrace.WriteToFile("MetaFitnessTrace-" + traceFilename);
            feasibleTrace.WriteToFile("MetaFeasibleTrace-" + traceFilename);

            Console.WriteLine("Press any key to exit ...");
            Console.ReadKey();
        }
    }
}
