using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class Franke : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public Franke(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "Franke";

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

            double term1 = 0.75 * Math.Exp(-Math.Pow((9 * x1 - 2), 2) / 4 - Math.Pow((9 * x2 - 2), 2) / 4);
            double term2 = 0.75 * Math.Exp(-Math.Pow((9 * x1 + 1), 2) / 49 - (9 * x2 + 1) / 10);
            double term3 = 0.5 * Math.Exp(-Math.Pow((9 * x1 - 7), 2) / 4 - Math.Pow((9 * x2 - 3), 2) / 4);
            double term4 = -0.2 * Math.Exp(-Math.Pow((9 * x1 - 4), 2) - Math.Pow((9 * x2 - 7), 2));
            return term1 + term2 + term3 + term4;
        }

        #endregion

    }
}
