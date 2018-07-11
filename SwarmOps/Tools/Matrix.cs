/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Portions (C) 2018 Matt R. Cole www.evolvedaisolutions.com
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// ------------------------------------------------------

namespace SwarmOps
{
    using EnsureThat;

    public static partial class Tools
    {
        /// <summary>
        /// Allocate and return a new matrix double[dim1][dim2].
        /// </summary>
        public static double[][] NewMatrix(int dim1, int dim2)
        {
            // TODO: the proper check here might be one
            Ensure.That(dim1).IsGte(0);
            Ensure.That(dim2).IsGte(0);
            double[][] matrix = new double[dim1][];

            for (int i = 0; i < dim1; i++)
            {
                matrix[i] = new double[dim2];
            }

            return matrix;
        }
    }
}
