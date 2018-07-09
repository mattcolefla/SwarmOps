using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class GoldsteinPrice : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public GoldsteinPrice(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "GoldsteinPrice";

        /// <summary>
        /// Minimum possible fitness.
        /// </summary>
        public override double MinFitness => -3;

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

            double fact1a = (int)(x1 + x2 + 1.0D) ^ 2;
            double fact1b = (int)(19 - 14 * x1 + 3 * x1) ^ 2 - (int)(14 * x2 + 6 * x1 * x2 + 3 * x2) ^ 2;
            double fact1 = 1 + fact1a * fact1b;
            double fact2a = (int)(2 * x1 - 3 * x2) ^ 2;
            double fact2b = (int)(18 - 32 * x1 + 12 * x1) ^ 2 + (int)(48 * x2 - 36 * x1 * x2 + 27 * x2) ^ 2;
            double fact2 = 30 + fact2a * fact2b;
            double y = fact1 * fact2;
            return (y);
        }

        #endregion

    }
}
