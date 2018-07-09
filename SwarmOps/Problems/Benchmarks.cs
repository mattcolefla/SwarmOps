/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// Portions copyright (C) 2018 Matt R. Cole
/// ------------------------------------------------------

using System;

namespace SwarmOps.Problems
{
    /// <summary>
    /// Contains list of all implemented benchmark problems.
    /// </summary>
    public static class Benchmarks
    {
        /// <summary>
        /// Enumeration of all benchmark problem IDs.
        /// </summary>
        public enum ID
        { 
            Ackley,
            Adjiman,
            BartelsConn,
            Beales,
            Bird,
            Branin,
            Bukin,
            Bumps,
            CrossInTray,
            CurrinExponential,
            DoubleDip,
            DropWave,
            Easom,
            EggHolder,
            FitnessTest,
            Franke,
            GoldsteinPrice,
            Grammacy,
            Griewank,
            Himmelblau,
            HolderTable,
            Hole,
            Levy,
            Levy13,
            Lim,
            ManyPeaks,
            Matyas,
            Mccormick,
            Michaelwicz,
            OO,
            Peaks,
            Penalized1, 
            Penalized2,
            Perm2,
            QuarticNoise, 
            Rastrigin, 
            Rosenbrock,
            RotatedHyperEllipsoid,
            Schaffer,
            Shubert,
            Schwefel12, 
            Schwefel221, 
            Schwefel222,
            SixHumpCamel,
            Sphere, 
            Step,
            SumOfSquares,
            Trid,
            Webster
        }

        /// <summary>
        /// Array containing all benchmark problem IDs.
        /// </summary>
        public static ID[] IDs =
        {
            ID.Ackley, 
            ID.Adjiman,
            ID.BartelsConn,
            ID.Beales,
            ID.Bird,
            ID.Branin,
            ID.Bukin,
            ID.Bumps,
            ID.CrossInTray,
            ID.CurrinExponential,
            ID.DoubleDip,
            ID.DropWave,
            ID.Easom,
            ID.EggHolder,
            ID.FitnessTest,
            ID.Franke,
            ID.GoldsteinPrice,
            ID.Grammacy,
            ID.Griewank, 
            ID.Himmelblau,
            ID.HolderTable,
            ID.Hole,
            ID.Levy,
            ID.Levy13,
            ID.Lim,
            ID.ManyPeaks,
            ID.Matyas,
            ID.Mccormick,
            ID.Michaelwicz,
            ID.OO,
            ID.Peaks,
            ID.Penalized1, 
            ID.Penalized2, 
            ID.Perm2,
            ID.QuarticNoise, 
            ID.Rastrigin, 
            ID.Rosenbrock,
            ID.RotatedHyperEllipsoid,
            ID.Schaffer,
            ID.Shubert,
            ID.Schwefel12, 
            ID.Schwefel221, 
            ID.Schwefel222,
            ID.SixHumpCamel,
            ID.Sphere, 
            ID.Step,
            ID.SumOfSquares,
            ID.Trid,
            ID.Webster
        };

        /// <summary>
        /// Create a new instance of a benchmark problem.
        /// </summary>
        /// <param name="id">BenchmarkProblem problem ID.</param>
        /// <param name="dimensionality">Dimensionality of problem.</param>
        /// <returns></returns>
        public static BenchmarkProblem CreateInstance(this ID id, int dimensionality, int maxIterations)
        {
            BenchmarkProblem benchmarkProblem;

            switch (id)
            {
                case ID.Ackley:
                    benchmarkProblem = new Ackley(dimensionality, maxIterations);
                    break;

                case ID.Griewank:
                    benchmarkProblem = new Griewank(dimensionality, maxIterations);
                    break;

                case ID.Penalized1:
                    benchmarkProblem = new Penalized1(dimensionality, maxIterations);
                    break;

                case ID.Penalized2:
                    benchmarkProblem = new Penalized2(dimensionality, maxIterations);
                    break;

                case ID.QuarticNoise:
                    benchmarkProblem = new QuarticNoise(dimensionality, maxIterations);
                    break;

                case ID.Rastrigin:
                    benchmarkProblem = new Rastrigin(dimensionality, maxIterations);
                    break;

                case ID.Rosenbrock:
                    benchmarkProblem = new Rosenbrock(dimensionality, maxIterations);
                    break;

                case ID.Schwefel12:
                    benchmarkProblem = new Schwefel12(dimensionality, maxIterations);
                    break;

                case ID.Schwefel221:
                    benchmarkProblem = new Schwefel221(dimensionality, maxIterations);
                    break;

                case ID.Schwefel222:
                    benchmarkProblem = new Schwefel222(dimensionality, maxIterations);
                    break;

                case ID.Sphere:
                    benchmarkProblem = new Sphere(dimensionality, maxIterations);
                    break;

                case ID.Step:
                    benchmarkProblem = new Step(dimensionality, maxIterations);
                    break;

                // MRC
                case ID.Lim:
                    benchmarkProblem = new Lim(dimensionality, maxIterations);
                    break;
                case ID.Trid:
                    benchmarkProblem = new Trid(dimensionality, maxIterations);
                    break;
                case ID.Shubert:
                    benchmarkProblem = new Shubert(dimensionality, maxIterations);
                    break;
                case ID.Matyas:
                    benchmarkProblem = new Matyas(dimensionality, maxIterations);
                    break;
                case ID.Branin:
                    benchmarkProblem = new Branin(dimensionality, maxIterations);
                    break;
                case ID.Franke:
                    benchmarkProblem = new Franke(dimensionality, maxIterations);
                    break;
                case ID.Grammacy:
                    benchmarkProblem = new Grammacy(dimensionality, maxIterations);
                    break;
                case ID.OO:
                    benchmarkProblem = new OO(dimensionality, maxIterations);
                    break;
                case ID.Webster:
                    benchmarkProblem = new Webster(dimensionality, maxIterations);
                    break;
                case ID.CurrinExponential:
                    benchmarkProblem = new CurrinExponential(dimensionality, maxIterations);
                    break;
                case ID.Adjiman:
                    benchmarkProblem = new Adjiman(dimensionality, maxIterations);
                    break;
                case ID.BartelsConn:
                    benchmarkProblem = new BartelsConn(dimensionality, maxIterations);
                    break;
                case ID.Bird:
                    benchmarkProblem = new Bird(dimensionality, maxIterations);
                    break;
                case ID.Himmelblau:
                    benchmarkProblem = new Himmelblau(dimensionality, maxIterations);
                    break;
                case ID.Perm2:
                    benchmarkProblem = new Perm2(dimensionality, maxIterations);
                    break;
                case ID.Schaffer:
                    benchmarkProblem = new Schaffer(dimensionality, maxIterations);
                    break;
                case ID.Michaelwicz:
                    benchmarkProblem = new Michaelwicz(dimensionality, maxIterations);
                    break;
                case ID.Easom:
                    benchmarkProblem = new Easom(dimensionality, maxIterations);
                    break;
                case ID.SixHumpCamel:
                    benchmarkProblem = new SixHumpCamel(dimensionality, maxIterations);
                    break;
                case ID.Levy:
                    benchmarkProblem = new Levy(dimensionality, maxIterations);
                    break;
                case ID.Mccormick:
                    benchmarkProblem = new Mccormick(dimensionality, maxIterations);
                    break;
                case ID.Levy13:
                    benchmarkProblem = new Levy13(dimensionality, maxIterations);
                    break;
                case ID.HolderTable:
                    benchmarkProblem = new HolderTable(dimensionality, maxIterations);
                    break;
                case ID.EggHolder:
                    benchmarkProblem = new EggHolder(dimensionality, maxIterations);
                    break;
                case ID.DropWave:
                    benchmarkProblem = new DropWave(dimensionality, maxIterations);
                    break;
                case ID.CrossInTray:
                    benchmarkProblem = new CrossInTray(dimensionality, maxIterations);
                    break;
                case ID.Bukin:
                    benchmarkProblem = new Bukin(dimensionality, maxIterations);
                    break;
                case ID.RotatedHyperEllipsoid:
                    benchmarkProblem = new RotatedHyperEllipsoid(dimensionality, maxIterations);
                    break;
                case ID.SumOfSquares:
                    benchmarkProblem = new SumOfSquares(dimensionality, maxIterations);
                    break;
                case ID.Beales:
                    benchmarkProblem = new Beales(dimensionality, maxIterations);
                    break;
                case ID.FitnessTest:
                    benchmarkProblem = new FitnessTest(dimensionality, maxIterations);
                    break;
                case ID.ManyPeaks:
                    benchmarkProblem = new ManyPeaks(dimensionality, maxIterations);
                    break;
                case ID.Hole:
                    benchmarkProblem = new Hole(dimensionality, maxIterations);
                    break;
                case ID.GoldsteinPrice:
                    benchmarkProblem = new GoldsteinPrice(dimensionality, maxIterations);
                    break;
                case ID.Bumps:
                    benchmarkProblem = new Bumps(dimensionality, maxIterations);
                    break;
                case ID.Peaks:
                    benchmarkProblem = new Peaks(dimensionality, maxIterations);
                    break;
                case ID.DoubleDip:
                    benchmarkProblem = new DoubleDip(dimensionality, maxIterations);
                    break;

                default:
                    throw new ArgumentException();
            }

            return benchmarkProblem;
        }
    }
}
