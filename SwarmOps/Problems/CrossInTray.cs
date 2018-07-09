﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class CrossInTray : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public CrossInTray(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "CrossInTray";

        /// <summary>
        /// Minimum possible fitness.
        /// </summary>
        public override double MinFitness => -20;

        /// <summary>
        /// Has the gradient has been implemented?
        /// </summary>
        public override bool HasGradient => false;

        /// <summary>
        /// Compute and return fitness for the given parameters.
        /// </summary>
        /// <param name="x">Candidate solution.</param>
        public override double Fitness(double[] x)
        {
            Debug.Assert(x != null && x.Length == Dimensionality);
            double x1 = x[0];
            double x2 = x[1];

            double fact1 = Math.Sin(x1) * Math.Sin(x2);
            double sqrt = Math.Sqrt((int)x1 ^ 2 + (int)x2 ^ 2);
            double fact2 = Math.Exp(Math.Abs(100 - sqrt / Math.PI));

            return -0.0001 * (Math.Abs(fact1 * fact2) + 1); //^ 0.1;
        }

        #endregion
    }
}
