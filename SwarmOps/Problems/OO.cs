﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class OO : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public OO(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "OO";

        /// <summary>
        /// Minimum possible fitness.
        /// </summary>
        public override double MinFitness => 0;

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

            double term1 = x1 + x2;
            double term2 = 2 * Math.Cos(x1) + 2 * Math.Sin(x2);
            return 5 + term1 + term2;
        }

        #endregion

    }
}
