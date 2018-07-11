/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// Portions copyright (C) 2018 Matt R. Cole www.evolvedaisolutions.com
/// ------------------------------------------------------

using System;
using SwarmOps;
using SwarmOps.Optimizers;
using SwarmOps.Problems;
using Console = Colorful.Console;


namespace TestParallelBenchmarks
{
    using System.Drawing;
    using NodaTime;

    /// <summary>
    /// Test a parallel optimizer on various benchmark problems.
    /// This is essentially the same as TestBenchmarks only the
    /// QuarticNoise problem cannot be used because it is not
    /// thread-safe, unless a thread-safe PRNG is used.
    /// A simulation of a time-consuming problem SphereSleep is
    /// also included here.
    /// </summary>
    class Program
    {
        // Create optimizer object.
        static readonly Optimizer Optimizer = new SwarmOps.Optimizers.Parallel.MOL();

        // Control parameters for optimizer.
        static readonly double[] Parameters = Optimizer.DefaultParameters;

        // Optimization settings.
        static readonly int NumRuns = 50;
        static readonly int Dim = 5;
        static readonly int DimFactor = 2000;
        static readonly int NumIterations = DimFactor * Dim;

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

            // Create a fitness trace for tracing the progress of optimization, mean.
            int NumMeanIntervals = 3000;
            FitnessTrace fitnessTraceMean = new FitnessTraceMean(NumIterations, NumMeanIntervals);

            // Create a fitness trace for tracing the progress of optimization, quartiles.
            // Note that fitnessTraceMean is chained to this object by passing it to the
            // constructor, this causes both fitness traces to be used.
            int NumQuartileIntervals = 10;
            FitnessTrace fitnessTraceQuartiles = new FitnessTraceQuartiles(NumRuns, NumIterations, NumQuartileIntervals, fitnessTraceMean);

            // Create a feasibility trace for tracing the progress of optimization re. fesibility.
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

            string msg = "{0} = {1} - {2} = {3} = {4} = {5} = {6} = {7} = {8} \r\n";
            // Output result-statistics.
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
            // Initialize PRNG.
            // If optimization problem doesn't use Globals.Random then it doesn't
            // have to be thread-safe. (Note that the Mangler uses the PRNG.)
            //Globals.Random = new RandomOps.MersenneTwister();
            // Otherwise use a fast and thread-safe PRNG, like so:
            Globals.Random = new RandomOps.ThreadSafe.CMWC4096();

            // Set max number of threads allowed.
            Globals.ParallelOptions.MaxDegreeOfParallelism = 8;

            // Output optimization settings.
            Console.WriteLine("BenchmarkProblem-tests. (Parallel)");
            Console.WriteLine("Optimizer: {0}", Optimizer.Name);
            Console.WriteLine("Using following parameters:");
            Tools.PrintParameters(Optimizer, Parameters);
            Console.WriteLine("Number of runs per problem: {0}", NumRuns);
            Console.WriteLine("Dimensionality: {0}", Dim);
            Console.WriteLine("Dim-factor: {0}", DimFactor);
            if (UseMangler)
            {
                Console.WriteLine("Mangle search-space:");
                Console.WriteLine("\tSpillover:     {0}", Spillover);
                Console.WriteLine("\tDisplacement:  {0}", Displacement);
                Console.WriteLine("\tDiffusion:     {0}", Diffusion);
                Console.WriteLine("\tFitnessNoise:  {0}", FitnessNoise);
            }
            else
            {
                Console.WriteLine("Mangle search-space: No");
            }
            Console.WriteLine();
            Console.WriteLine("Problem = Mean = Std.Dev. = Min = Q1 = Median = Q3 = Max = Feasible");
            Console.WriteLine("");

            // Start-time.
            ZonedDateTime t1 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();

            // Simulates a time-consuming optimization problem.
            //Optimize(new SphereSleep(1, Dim, NumIterations));

            // Thread-safe benchmark problems.
            Optimize(new Ackley(Dim, NumIterations));
            Optimize(new Griewank(Dim, NumIterations));
            Optimize(new Penalized1(Dim, NumIterations));
            Optimize(new Penalized2(Dim, NumIterations));
            Optimize(new Rastrigin(Dim, NumIterations));
            Optimize(new Rosenbrock(Dim, NumIterations));
            Optimize(new Schwefel12(Dim, NumIterations));
            Optimize(new Schwefel221(Dim, NumIterations));
            Optimize(new Schwefel222(Dim, NumIterations));
            Optimize(new Sphere(Dim, NumIterations));
            Optimize(new Step(Dim, NumIterations));

            // MRC
            Optimize(new Lim(Dim, NumIterations));
            Optimize(new Trid(Dim, NumIterations));
            Optimize(new Shubert(Dim, NumIterations));
            Optimize(new Matyas(Dim, NumIterations));
            Optimize(new Branin(Dim, NumIterations));
            Optimize(new Franke(Dim, NumIterations));
            Optimize(new Grammacy(Dim, NumIterations));
            Optimize(new OO(Dim, NumIterations));
            Optimize(new Webster(Dim, NumIterations));
            Optimize(new CurrinExponential(Dim, NumIterations));
            Optimize(new Adjiman(Dim, NumIterations));
            Optimize(new BartelsConn(Dim, NumIterations));
            Optimize(new Bird(Dim, NumIterations));
            Optimize(new Himmelblau(Dim, NumIterations));
            Optimize(new Perm2(Dim, NumIterations));

            Optimize(new Schaffer(Dim, NumIterations));
            Optimize(new Michaelwicz(Dim, NumIterations));
            Optimize(new Easom(Dim, NumIterations));
            Optimize(new SixHumpCamel(Dim, NumIterations));
            Optimize(new Levy(Dim, NumIterations));
            Optimize(new Mccormick(Dim, NumIterations));
            Optimize(new Levy13(Dim, NumIterations));
            Optimize(new HolderTable(Dim, NumIterations));
            Optimize(new EggHolder(Dim, NumIterations));
            Optimize(new DropWave(Dim, NumIterations));
            Optimize(new CrossInTray(Dim, NumIterations));

            // BenchmarkProblem problem using Globals.Random (see note above.)
            //Optimize(new QuarticNoise(Dim, NumIterations));

            // End-time.
            ZonedDateTime t2 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();
            Duration diff = t2.ToInstant() - t1.ToInstant();


            // Output time-usage.
            Console.WriteLine();
            Console.WriteLine("Time usage: {0}", t2 - t1);
        }
    }
}
