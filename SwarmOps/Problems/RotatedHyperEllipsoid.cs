using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class RotatedHyperEllipsoid : Benchmark
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public RotatedHyperEllipsoid(int dimensionality, int maxIterations)
            : base(dimensionality, -66, 66, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "RotatedHyperEllipsoid";

        /// <summary>
        /// Minimum possible fitness.
        /// </summary>
        public override double MinFitness => -66;

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
            int d = x.Length;
            double outer = 0D;
            double inner = 0D;

            for (int ii = 0; ii < d; ii++)
            {
                for (int jj = 0; jj < ii; jj++)
                {
                    double xj = x[jj];
                    inner = Convert.ToInt32(inner + xj) ^ 2;
                }
                outer = outer + inner;
            }

            return outer;
        }

        #endregion
    }
}
