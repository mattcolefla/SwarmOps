
using System;
using System.Collections.Generic;
using System.Linq;

public abstract class Problem
{
	public int dimension;

	public Problem(int dimension)
	{
		this.dimension = dimension;
	}
	public abstract double eval(List<double> solution);
}

public class Position
{
	public double fitness;
	public List<double> solution = new List<double>();

	public Position()
	{
	}
	public Position(List<double> solution, double fitness)
	{
		this.solution = solution;
		this.fitness = fitness;
	}

	public static bool operator == (Position p1, Position p2)
	{
	    return !p1.solution.Where((t, i) => t != p2.solution[i]).Any();
	}

    public static bool operator !=(Position p1, Position p2)
    {
        return !p1.solution.Where((t, i) => t == p2.solution[i]).Any();
    }

    public static double operator - (Position p1, Position p2)
    {
        return p1.solution.Select((t, i) => Math.Abs(t - p2.solution[i])).Sum();
    }

	public static Position init_position(Problem problem)
	{
		double[] solution = new double[problem.dimension];
		for (int i = 0; i < problem.dimension; ++i)
		{
			solution[i] = GlobalMembers.randu() * 200.0f - 100.0f;
		}
		return new Position(solution.ToList(), 1E100);
	}
}

public class Vibration
{
	public double intensity;
	public Position position = new Position();
	public static double C = -1E-100;

	public Vibration()
	{
	}
	public Vibration(Position position)
	{
		intensity = fitness_to_intensity(position.fitness);
		this.position = position;

	}
	public Vibration(double intensity, Position position)
	{
		this.intensity = intensity;
		this.position = position;

	}

	public double intensity_attenuation(double attenuation_factor, double distance)
	{
		return intensity * Math.Exp(-distance / attenuation_factor);
	}
	public static double fitness_to_intensity(double fitness)
	{
		return Math.Log(1.0 / (fitness - C) + 1.0);
	}
}

public class Spider
{
	public Position position = new Position();
	public Vibration target_vibr = new Vibration();
	public List<bool> dimension_mask = new List<bool>();
	public List<double> previous_move = new List<double>();
	public int inactive_deg;

	public Spider(Position position)
	{
		this.position = position;
		inactive_deg = 0;
		target_vibr = new Vibration(0, position);
		dimension_mask.Resize(position.solution.Count);
		previous_move.Resize(position.solution.Count);
	}

	public void choose_vibration(List<Vibration> vibrations, List<double> distances, double attenuation_factor)
	{
		int max_index = -1;
		double max_intensity = target_vibr.intensity;
		for (int i = 0; i < vibrations.Count; ++i)
		{
			if (vibrations[i].position == target_vibr.position)
			{
				continue;
			}

			double intensity = vibrations[i].intensity_attenuation(attenuation_factor, distances[i]);
			if (intensity > max_intensity)
			{
				max_index = i;
				max_intensity = intensity;
			}
		}
		if (max_index != -1)
		{
			target_vibr = new Vibration(max_intensity, vibrations[max_index].position);
			inactive_deg = 0;
		}
		else
		{
			++inactive_deg;
		}
	}
	public void mask_changing(double p_change, double p_mask)
	{
		if (GlobalMembers.randu() > Math.Pow(p_change, inactive_deg))
		{
			inactive_deg = 0;
			p_mask *= GlobalMembers.randu();
			for (int i = 0; i < dimension_mask.Count; ++i)
			{
				dimension_mask[i] = (GlobalMembers.randu()) < p_mask;
			}
		}
	}
	public void random_walk(List<Vibration> vibrations)
	{
		for (int i = 0; i < position.solution.Count; ++i)
		{
			previous_move[i] *= GlobalMembers.randu();
			double target_position = dimension_mask[i] ? vibrations[RandomNumbers.NextNumber() % vibrations.Count].position.solution[i] : target_vibr.position.solution[i];
			previous_move[i] += GlobalMembers.randu() * (target_position - position.solution[i]);
			position.solution[i] += previous_move[i];
		}
	}
}

public class SSA
{
	public Problem problem;
	public int dimension;
	public List<Spider> population = new List<Spider>();
	public List<Vibration> vibrations = new List<Vibration>();
	public List<List<double>> distances = new List<List<double>>();
	public Position global_best = new Position();

	public SSA(Problem problem, int pop_size)
	{
		this.problem = problem;
		dimension = problem.dimension;
		RandomNumbers.Seed(1);
		population.Capacity = pop_size;
		distances.Resize(pop_size);

		for (int i = 0; i < pop_size; ++i)
		{
			Position position = Position.init_position(problem);
			population.Add(new Spider(position));
            distances[i] = new List<double>();
			distances[i].Resize(pop_size);
		}
		vibrations.Capacity = pop_size;
		global_best = population[0].position;
	}

	public void run(int max_iteration, double attenuation_rate, double p_change, double p_mask)
	{
		print_header();
		start_time = DateTime.Now;
		for (iteration = 1; iteration <= max_iteration; ++iteration)
		{
			fitness_calculation();
			vibration_generation(attenuation_rate);
			foreach (var t in population)
			{
			    t.mask_changing(p_change, p_mask);
			    t.random_walk(vibrations);
			}
			if ((iteration == 1) || (iteration == 10) || (iteration < 1001 && iteration % 100 == 0) || (iteration < 10001 && iteration % 1000 == 0) || (iteration < 100001 && iteration % 10000 == 0))
			{
				print_content();
			}
		}
		iteration--;
		print_footer();
	}
	public void fitness_calculation()
	{
		population_best_fitness = 1E100;
		foreach (var t in population)
		{
		    double fitness = problem.eval(t.position.solution);
		    t.position.fitness = fitness;
		    if (fitness > global_best.fitness)
		    {
		        global_best = t.position;
		    }
		    if (fitness > population_best_fitness)
		    {
		        population_best_fitness = fitness;
		    }
		}
		mean_distance = 0;
		for (int i = 0; i < population.Count; ++i)
		{
			for (int j = i + 1; j < population.Count; ++j)
			{
				distances[i][j] = population[i].position - population[j].position;
				distances[j][i] = distances[i][j];
				mean_distance += distances[i][j];
			}
		}
		mean_distance /= (population.Count * (population.Count - 1) / 2);
	}
	public void vibration_generation(double attenuation_rate)
	{
		vibrations.Clear();
		foreach (var t in population)
		{
		    vibrations.Add(new Vibration(t.position));
		}
		double sum = 0.0;
		List<double> data = new List<double>();
		data.Resize(population.Count);
		for (int i = 0; i < dimension; ++i)
		{
			for (int j = 0; j < population.Count; ++j)
			{
				data[j] = population[j].position.solution[i];
			}

			sum += GlobalMembers.std_dev(data);
		}
		attenuation_base = sum / dimension;
		for (int i = 0; i < population.Count; ++i)
		{
			population[i].choose_vibration(vibrations, distances[i], attenuation_base * attenuation_rate);
		}
	}

	private int iteration;
	private double population_best_fitness;
	private double attenuation_base;
	private double mean_distance;
	private DateTime start_time = DateTime.Now;

	private void print_header()
	{
		Console.Write("               SSA starts at ");
		Console.Write(GlobalMembers.get_time_string());
		Console.Write("\n");
		Console.Write("==============================================================");
		Console.Write("\n");
		Console.Write(" iter\tbest fitness\tpop_fitness\tbase_dist\tmean_dist\ttime_elapsed");
		Console.Write("\n");
		Console.Write("==============================================================");
		Console.Write("\n");
	}
	private void print_content()
	{
		var current_time = DateTime.Now;
		Console.Write("{0,5:D}\t{1:e3}\t{2:e3}\t{3:e3}\t{4:e3} ", 
		    iteration, global_best.fitness, population_best_fitness, attenuation_base, mean_distance);
		Console.Write(GlobalMembers.get_time_string(current_time - start_time));
		Console.Write("\n");
	}
	private void print_footer()
	{
		Console.Write("==============================================================");
		Console.Write("\n");
		print_content();
		Console.Write("==============================================================");
		Console.Write("\n");
	}
}