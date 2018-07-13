using System;
using System.Collections.Generic;

public static class GlobalMembers
{
	static int Main()
	{
		SSA ssa = new SSA(new MyProblem(30), 30);
		ssa.run(10000, 1.0, 0.7, 0.1);
        Console.WriteLine("Finished");
	    Console.ReadLine();
		return 0;
	}

	public static double mean(List<double> data)
	{
		double sum = 0.0;
		foreach (var t in data)
		{
		    sum += t;
		}
		return sum / data.Count;
	}

	public static double std_dev(List<double> data)
	{
		double mean_val = mean(data);
		double sum = 0.0;
		foreach (var t in data)
		{
		    sum += (mean_val - t) * (mean_val - t);
		}
		return Math.Sqrt(sum / data.Count);
	}

	public static double randu()
	{
        return (double)RandomNumbers.NextNumber() / (32767 + 1.0f);
	}

	public static string get_time_string()
	{
        return DateTime.Now.ToString("%Y-%m-%d %H:%M:%S");
	}
	public static string get_time_string(TimeSpan ts)
	{
		return $"{ts.TotalSeconds} seconds";
	}
}