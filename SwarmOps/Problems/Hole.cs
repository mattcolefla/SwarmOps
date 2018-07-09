using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class Hole : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public Hole(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "Hole";

        /// <summary>
        /// Minimum possible fitness.
        /// </summary>
        public override double MinFitness => -10;

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
            x[0] = (x[0] + 3) / 6d;   // [0,1]
            x[1] = (x[1] + 3) / 6d;   // [0,1]
            x[0] = 10 * x[0] - 5;
            x[1] = 10 * x[1] - 5;

            double z = Math.Cos(x[0]) * Math.Cos(x[1]);
            z *= z * z;
            return 1 - z;
        }

        #endregion

    }
}
