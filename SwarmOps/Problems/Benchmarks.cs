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
        /// <param name="id">Benchmark problem ID.</param>
        /// <param name="dimensionality">Dimensionality of problem.</param>
        /// <returns></returns>
        public static Benchmark CreateInstance(this ID id, int dimensionality, int maxIterations)
        {
            Benchmark benchmark;

            switch (id)
            {
                case ID.Ackley:
                    benchmark = new Ackley(dimensionality, maxIterations);
                    break;

                case ID.Griewank:
                    benchmark = new Griewank(dimensionality, maxIterations);
                    break;

                case ID.Penalized1:
                    benchmark = new Penalized1(dimensionality, maxIterations);
                    break;

                case ID.Penalized2:
                    benchmark = new Penalized2(dimensionality, maxIterations);
                    break;

                case ID.QuarticNoise:
                    benchmark = new QuarticNoise(dimensionality, maxIterations);
                    break;

                case ID.Rastrigin:
                    benchmark = new Rastrigin(dimensionality, maxIterations);
                    break;

                case ID.Rosenbrock:
                    benchmark = new Rosenbrock(dimensionality, maxIterations);
                    break;

                case ID.Schwefel12:
                    benchmark = new Schwefel12(dimensionality, maxIterations);
                    break;

                case ID.Schwefel221:
                    benchmark = new Schwefel221(dimensionality, maxIterations);
                    break;

                case ID.Schwefel222:
                    benchmark = new Schwefel222(dimensionality, maxIterations);
                    break;

                case ID.Sphere:
                    benchmark = new Sphere(dimensionality, maxIterations);
                    break;

                case ID.Step:
                    benchmark = new Step(dimensionality, maxIterations);
                    break;

                // MRC
                case ID.Lim:
                    benchmark = new Lim(dimensionality, maxIterations);
                    break;
                case ID.Trid:
                    benchmark = new Trid(dimensionality, maxIterations);
                    break;
                case ID.Shubert:
                    benchmark = new Shubert(dimensionality, maxIterations);
                    break;
                case ID.Matyas:
                    benchmark = new Matyas(dimensionality, maxIterations);
                    break;
                case ID.Branin:
                    benchmark = new Branin(dimensionality, maxIterations);
                    break;
                case ID.Franke:
                    benchmark = new Franke(dimensionality, maxIterations);
                    break;
                case ID.Grammacy:
                    benchmark = new Grammacy(dimensionality, maxIterations);
                    break;
                case ID.OO:
                    benchmark = new OO(dimensionality, maxIterations);
                    break;
                case ID.Webster:
                    benchmark = new Webster(dimensionality, maxIterations);
                    break;
                case ID.CurrinExponential:
                    benchmark = new CurrinExponential(dimensionality, maxIterations);
                    break;
                case ID.Adjiman:
                    benchmark = new Adjiman(dimensionality, maxIterations);
                    break;
                case ID.BartelsConn:
                    benchmark = new BartelsConn(dimensionality, maxIterations);
                    break;
                case ID.Bird:
                    benchmark = new Bird(dimensionality, maxIterations);
                    break;
                case ID.Himmelblau:
                    benchmark = new Himmelblau(dimensionality, maxIterations);
                    break;
                case ID.Perm2:
                    benchmark = new Perm2(dimensionality, maxIterations);
                    break;
                case ID.Schaffer:
                    benchmark = new Schaffer(dimensionality, maxIterations);
                    break;
                case ID.Michaelwicz:
                    benchmark = new Michaelwicz(dimensionality, maxIterations);
                    break;
                case ID.Easom:
                    benchmark = new Easom(dimensionality, maxIterations);
                    break;
                case ID.SixHumpCamel:
                    benchmark = new SixHumpCamel(dimensionality, maxIterations);
                    break;
                case ID.Levy:
                    benchmark = new Levy(dimensionality, maxIterations);
                    break;
                case ID.Mccormick:
                    benchmark = new Mccormick(dimensionality, maxIterations);
                    break;
                case ID.Levy13:
                    benchmark = new Levy13(dimensionality, maxIterations);
                    break;
                case ID.HolderTable:
                    benchmark = new HolderTable(dimensionality, maxIterations);
                    break;
                case ID.EggHolder:
                    benchmark = new EggHolder(dimensionality, maxIterations);
                    break;
                case ID.DropWave:
                    benchmark = new DropWave(dimensionality, maxIterations);
                    break;
                case ID.CrossInTray:
                    benchmark = new CrossInTray(dimensionality, maxIterations);
                    break;
                case ID.Bukin:
                    benchmark = new Bukin(dimensionality, maxIterations);
                    break;
                case ID.RotatedHyperEllipsoid:
                    benchmark = new RotatedHyperEllipsoid(dimensionality, maxIterations);
                    break;
                case ID.SumOfSquares:
                    benchmark = new SumOfSquares(dimensionality, maxIterations);
                    break;
                case ID.Beales:
                    benchmark = new Beales(dimensionality, maxIterations);
                    break;
                case ID.FitnessTest:
                    benchmark = new FitnessTest(dimensionality, maxIterations);
                    break;
                case ID.ManyPeaks:
                    benchmark = new ManyPeaks(dimensionality, maxIterations);
                    break;
                case ID.Hole:
                    benchmark = new Hole(dimensionality, maxIterations);
                    break;
                case ID.GoldsteinPrice:
                    benchmark = new GoldsteinPrice(dimensionality, maxIterations);
                    break;
                case ID.Bumps:
                    benchmark = new Bumps(dimensionality, maxIterations);
                    break;

                    
                default:
                    throw new ArgumentException();
            }

            return benchmark;
        }
    }
}
