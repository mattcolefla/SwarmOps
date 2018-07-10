﻿/// ------------------------------------------------------
/// SwarmOps - Numeric and heuristic optimization for C#
/// Copyright (C) 2003-2011 Magnus Erik Hvass Pedersen.
/// Please see the file license.txt for license details.
/// SwarmOps on the internet: http://www.Hvass-Labs.org/
/// Portions copyright (C) 2018 Matt R. Cole 
/// ------------------------------------------------------

using System;
using System.Diagnostics;
using Console = Colorful.Console;

namespace SwarmOps
{
    using System.Drawing;

    public static partial class Tools
    {
        /// <summary>
        /// Print parameters using names associated with an optimization problem.
        /// </summary>
        /// <param name="problem">Optimization problem with associated parameter-names.</param>
        /// <param name="parameters">Parameters to be printed.</param>
        public static void PrintParameters(Problem problem, double[] parameters)
        {
            int NumParameters = problem.Dimensionality;

            if (NumParameters > 0)
            {
                Debug.Assert(parameters.Length == NumParameters);

                for (int i = 0; i < NumParameters; i++)
                {
                    string parameterName = problem.ParameterName[i];
                    double p = parameters[i];
                    string pStr = p.ToString("0.####", _cultureInfo);

                    Console.WriteLine("\t{0} = {1}", parameterName, pStr, Color.LightBlue);
                }
            }
            else
            {
                Console.WriteLine("\tN/A", Color.Red);
            }
        }

        /// <summary>
        /// Print parameters, fitness and feasibility to Console, and print a marking if
        /// fitness was an improvement to fitnessLimit.
        /// </summary>
        public static void PrintSolution(double[] parameters, double fitness, double fitnessLimit, bool oldFeasible, bool newFeasible, bool formatAsArray)
        {
            // Convert parameters to a string.
            string parametersStr = (formatAsArray) ? (ArrayToString(parameters)) : (ArrayToStringRaw(parameters));


            Console.WriteLine("{0} \t{1} \t{2} {3}",
                parametersStr,
                FormatNumber(fitness),
                (newFeasible) ? (1) : (0),
                BetterFeasibleFitness(oldFeasible, newFeasible, fitnessLimit, fitness) ? ("***") : (""),
                BetterFeasibleFitness(oldFeasible, newFeasible, fitnessLimit, fitness) ? Color.Green : Color.LightBlue);

            // Flush stdout, this is useful if piping the output and you wish
            // to study the the output before the entire optimization run is complete.
            Console.Out.Flush();
        }

        /// <summary>
        /// Print a newline to Console.
        /// </summary>
        public static void PrintNewline()
        {
            Console.WriteLine();
            Console.Out.Flush();
        }
    }
}
