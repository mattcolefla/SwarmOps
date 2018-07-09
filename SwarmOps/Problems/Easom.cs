﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class Easom : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public Easom(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "Easom";

        /// <summary>
        /// Minimum possible fitness.
        /// </summary>
        public override double MinFitness => -100;

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

            double fact1 = -Math.Cos(x1) * Math.Cos(x2);
            double fact2 = Math.Exp(-(int)(x1 - Math.PI) ^ 2 - (int)(x2 - Math.PI) ^ 2);
            return fact1 * fact2;
        }

        #endregion
    }
}
