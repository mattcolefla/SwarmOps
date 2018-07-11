/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Portions (C) 2018 Matt R. Cole www.evolvedaisolutions.com
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// ------------------------------------------------------

using System.Diagnostics;

namespace SwarmOps
{
    using EnsureThat;

    public static partial class Tools
    {
        /// <summary>
        /// Bound a value between lower and upper boundaries.
        /// </summary>
        /// <param name="x">Parameter to be bounded.</param>
        /// <param name="lower">Lower boundary.</param>
        /// <param name="upper">Upper boundary.</param>
        /// <returns>Bounded value.</returns>
        public static double Bound(double x, double lower, double upper)
        {
            //Ensure.That(x).IsGte(0);
            //Ensure.That(lower).IsGte(0);
            Ensure.That(upper).IsGte(lower);

            double y;

            if (x < lower)
            {
                y = lower;
            }
            else if (x > upper)
            {
                y = upper;
            }
            else
            {
                y = x;
            }

            return y;
        }

        /// <summary>
        /// Bound array of values between lower and upper boundaries.
        /// </summary>
        /// <param name="x">Array of values to be bounded.</param>
        /// <param name="lower">Lower boundaries.</param>
        /// <param name="upper">Upper boundaries.</param>
        public static void Bound(ref double[] x, double[] lower, double[] upper)
        {
            Ensure.That(x).SizeIs(lower.Length);
            Ensure.That(x).SizeIs(upper.Length);

            for (int i = 0; i < x.Length; i++)
            {
                x[i] = Bound(x[i], lower[i], upper[i]);
            }
        }

        /// <summary>
        /// Return whether array of values is between lower and upper boundaries.
        /// </summary>
        /// <param name="x">Array of values to be bounded.</param>
        /// <param name="lower">Lower boundaries.</param>
        /// <param name="upper">Upper boundaries.</param>
        public static bool BetweenBounds(double[] x, double[] lower, double[] upper)
        {
            Ensure.That(x).SizeIs(lower.Length);
            Ensure.That(x).SizeIs(upper.Length);

            bool retVal = true;

            for (int i = 0; retVal && i < x.Length; i++)
            {
                retVal = retVal && x[i] >= lower[i] && x[i] <= upper[i]; 
            }

            return retVal;
        }
    }
}
