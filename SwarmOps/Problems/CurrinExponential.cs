using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class CurrinExponential : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public CurrinExponential(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "CurrinExponential";

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

            double fact1 = 1 - Math.Exp(-1 / (2 * x2));
            double fact2 = Math.Pow(2300 * x1, 3) + Math.Pow(1900 * x1, 2) + 2092 * x1 + 60;
            double fact3 = Math.Pow(100 * x1, 3) + Math.Pow(500 * x1, 2) + 4 * x1 + 20;
            return fact1 * fact2 / fact3;
        }

        #endregion

    }
}
