

using System.Collections.Generic;

public class MyProblem : Problem
{
	public MyProblem(int dimension) : base(dimension)
	{
	}

	public override double eval(List<double> solution)
	{
		double sum = 0.0;
		foreach (var t in solution)
		{
		    sum += t * t;
		}
		return sum;
	}
}