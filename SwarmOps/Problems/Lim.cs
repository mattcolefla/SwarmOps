using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmOps;
using SwarmOps.Problems;

namespace SwarmOps.Problems
{
    public class Lim : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public Lim(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "Lim";

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

            double term1 = (5 / 2) * x1 - (35 / 2) * x2;
            double term2 = Math.Pow((5 / 2) * x1 * x2 + 19 * x2, 2);
            double term3 = -Math.Pow((15 / 2) * x1, 3) - Math.Pow((5 / 2) * x1 * x2, 2);
            double term4 = -Math.Pow((11 / 2) * x2, 4) + (Math.Pow(x1, 3)) * (Math.Pow(x2, 2));
            return 9 + term1 + term2 + term3 + term4;
        }

        #endregion
    }
}