using System;

class OperationsCount
{
	void Main(string[] args)
	{
		int N = 0;
		foreach(var arg in args)
		{
			var words = arg.Split(":");
			if(words[0] == "-N") N = int.Parse(words[1]);
		}
		double[] xs = new double[N];
		double[] ys = new double[N];
		
	}
}
