using System;
using static System.Console;
using static System.Math;

public static class main
{
	public static void Main(string[] args)
	{
		char[] delimiters = {' ','\t', '\n'};
		var options = StringSplitOptions.RemoveEmptyEntries;
		for(string line = ReadLine(); line != null; line = ReadLine())
		{
			var numbers = line.Split(delimiters,options);
			foreach(var number in numbers)
			{
				double x = double.Parse(number);
				Error.WriteLine($"{x} {Sin(x)} {Cos(x)}");
			}
		} 
	}
}
