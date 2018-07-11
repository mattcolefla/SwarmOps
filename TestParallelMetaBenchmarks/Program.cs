/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// Portions copyright (C) 2018 Matt R. Cole www.evolvedaisolutions.com
/// ------------------------------------------------------

using System;
using System.Collections.Generic;

using SwarmOps;
using SwarmOps.Problems;
using SwarmOps.Optimizers;
using Console = Colorful.Console;


namespace TestParallelMetaBenchmarks
{
    using System.Drawing;
    using NodaTime;

    /// <summary>
    /// Similar to TestMetaBenchmark only using the parallel version
    /// of MetaFitness and a thread-safe PRNG. Search-space mangler
    /// is not used because it is incompatible with parallel MetaFitness.
    /// </summary>
    class Program
    {
        // Settings for the optimization layer.
        static readonly int NumRuns = 64;       // Set this close to 50 and a multiple of the number of processors, e.g. 8.
        static readonly int Dim = 5;
        static readonly int DimFactor = 2000;
        static readonly int NumIterations = DimFactor * Dim;

        // The optimizer whose control parameters are to be tuned.
        static Optimizer Optimizer = new MOL();

        // Problems to optimize. That is, the optimizer is having its control
        // parameters tuned to work well on these problems. The numbers are weights
        // that signify mutual importance of the problems in tuning. Higher weight
        // means more importance.
        static WeightedProblem[] WeightedProblems =
            new WeightedProblem[]
            {
                //new WeightedProblem(1.0, new Ackley(Dim, NumIterations)),
                //new WeightedProblem(1.0, new Griewank(Dim, NumIterations)),
                new WeightedProblem(1.0, new Penalized1(Dim, NumIterations)),
                //new WeightedProblem(1.0, new Penalized2(Dim, NumIterations)),
                //new WeightedProblem(1.0, new QuarticNoise(Dim, NumIterations)),
                //new WeightedProblem(1.0, new Rastrigin(Dim, NumIterations)),
                new WeightedProblem(1.0, new Rosenbrock(Dim, NumIterations)),
                new WeightedProblem(1.0, new Schwefel12(Dim, NumIterations)),
                //new WeightedProblem(1.0, new Schwefel221(Dim, NumIterations)),
                //new WeightedProblem(1.0, new Schwefel222(Dim, NumIterations)),
                new WeightedProblem(1.0, new Sphere(Dim, NumIterations)),
                //new WeightedProblem(1.0, new Step(Dim, NumIterations)),
            };

        // Settings for the meta-optimization layer.
        static readonly int MetaNumRuns = 5;
        static readonly int MetaDim = Optimizer.Dimensionality;
        static readonly int MetaDimFactor = 20;
        static readonly int MetaNumIterations = MetaDimFactor * MetaDim;

        // The meta-fitness consists of computing optimization performance
        // for the problems listed above over several optimization runs and
        // sum the results, so we wrap the Optimizer-object in a
        // MetaFitness-object which takes of this.
        static SwarmOps.Optimizers.Parallel.MetaFitness MetaFitness = new SwarmOps.Optimizers.Parallel.MetaFitness(Optimizer, WeightedProblems, NumRuns, MetaNumIterations);

        // Print meta-optimization progress.
        static FitnessPrint MetaFitnessPrint = new FitnessPrint(MetaFitness);

        // Log all candidate solutions.
        static int LogCapacity = 20;
        static bool LogOnlyFeasible = true;
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
            // Parallel version uses a ThreadSafe PRNG.
            Globals.Random = new RandomOps.ThreadSafe.CMWC4096();

            // Set max number of threads allowed.
            Globals.ParallelOptions.MaxDegreeOfParallelism = 8;

            // Create a fitness trace for tracing the progress of meta-optimization.
            int MaxMeanIntervals = 3000;
            FitnessTrace fitnessTrace = new FitnessTraceMean(MetaNumIterations, MaxMeanIntervals);
            FeasibleTrace feasibleTrace = new FeasibleTrace(MetaNumIterations, MaxMeanIntervals, fitnessTrace);

            // Assign the fitness trace to the meta-optimizer.
            MetaOptimizer.FitnessTrace = feasibleTrace;

            // Output settings.
            Console.WriteLine("Meta-Optimization of benchmark problems. (Parallel)", Color.Yellow);
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
            Console.WriteLine("(Search-space mangling not supported for parallel meta-optimization.)", Color.Yellow);
            Console.WriteLine();

            Console.WriteLine("0/1 Boolean whether optimizer's control parameters are feasible.", Color.Yellow);
            Console.WriteLine("*** Indicates meta-fitness/feasibility is an improvement.", Color.Yellow);

            // Start-time.
            ZonedDateTime t1 = LocalDateTime.FromDateTime(DateTime.Now).InUtc();

            // Perform the meta-optimization runs.
            double fitness = MetaRepeat.Fitness(MetaParameters);

            // End-time.
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
            Console.WriteLine("Time usage: {0}", t2 - t1);

            // Output fitness and feasible trace.
            string traceFilename
                = MetaOptimizer.Name + "-" + Optimizer.Name
                + "-" + WeightedProblems.Length + "Bnch" + "-" + DimFactor + "xDim.txt";
            fitnessTrace.WriteToFile("MetaFitnessTrace-" + traceFilename);
            feasibleTrace.WriteToFile("MetaFeasibleTrace-" + traceFilename);

            //Console.WriteLine("Press any key to exit ...");
            //Console.ReadKey();
        }
    }
}
