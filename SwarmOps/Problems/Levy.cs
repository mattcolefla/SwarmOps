using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmOps.Problems
{
    using System.Diagnostics;

    public class Levy : BenchmarkProblem
    {
        #region Constructors.

        /// <summary>
        /// Construct the object.
        /// </summary>
        /// <param name="dimensionality">Dimensionality of the problem (e.g. 20)</param>
        /// <param name="maxIterations">Max optimization iterations to perform.</param>
        public Levy(int dimensionality, int maxIterations)
            : base(dimensionality, -10, 10, 0, 0, 100)
        {
        }

        #endregion

        #region Base-class overrides.

        /// <summary>
        /// Name of the optimization problem.
        /// </summary>
        public override string Name => "Levy";

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
            int d = x.Length;
            List<double> w = new List<double>();

            for (int ii = 0; ii < d; ii++)
            {
                w.Add(1 + (x[ii] - 1) / 4);
            }

            double term1 = (int)(Math.Sin(Math.PI * w[0])) ^ 2;
            double term3 = ((int)(w[d-1] - 1)) ^ 2 * (1 + (int)(Math.Sin(2 * Math.PI * w[d-1])) ^ 2);

            double sum = 0;
            for (int ii = 0; ii < d - 1; ii++)
            {
                double wi = w[ii];
                double n = (int)(wi - 1) ^ 2 * (1 + 10 * (int)(Math.Sin(Math.PI * wi + 1)) ^ 2);
                sum = sum + n;
            }

            return term1 + sum + term3;
        }

        #endregion
    }
}
