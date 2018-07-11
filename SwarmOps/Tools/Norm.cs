/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Portions (C) 2018 Matt R. Cole www.evolvedaisolutions.com
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// ------------------------------------------------------

using System.Linq;

namespace SwarmOps
{
    using EnsureThat;

    public static partial class Tools
    {
        /// <summary>
        /// Euclidian norm (or length) of a numeric vector.
        /// </summary>
        public static double Norm(double[] x)
        {
            Ensure.That(x).IsNotNull();

            return System.Math.Sqrt(x.Aggregate((double)0.0, (sum, elm) => sum += elm * elm));
        }
    }
}
