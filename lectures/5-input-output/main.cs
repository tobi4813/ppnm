using System;
using static System.Console;
using static System.Math;

class main
{
	public static int Main(string[] args)
	{
		Write("Yello\n");
		string infile = null, outfile=null;
		foreach(string arg in args) 
		{
			System.Console.Out.WriteLine(arg); //System.Console.Write = System.Console.Out.Write
			var words = arg.Split(':');
			if(words[0]=="-input") infile = words[1];
			if(words[0]=="-output") outfile = words[1];
		}
		if(infile == null) Error.WriteLine("no input file"); return 1;
		double[] numbers = input.get_numbers_from_args(args);
		foreach(double number in numbers) System.Console.Out.WriteLine($"{number:0.00e+00}");
		System.Console.Error.WriteLine("return code 0");
		var inputstream = new System.IO.StreamReader(infile);
		var outputstream = new System.IO.StreamWriter(outfile, append:false);
		
		for(string line = inputstream.ReadLine(); line != null; line=inputstream.ReadLine())
		{
			double x = double.Parse(line);
			outputstream.WriteLine($"{x} {Sin(x)}");
		}
		inputstream.Close();
		outputstream.Close();
		for(string line=In.ReadLine(); line!=null; In.ReadLine())
		{
			double x=double.Parse(line);
			Out.WriteLine($"{x} {Sin(x)}");
		}
		return 0;
	}
}
