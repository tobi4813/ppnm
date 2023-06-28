using System;
using System.Threading.Tasks;
using static System.Console;

static class parallel
{
	static void Main()
	{
		double sum=0; 
		Parallel.For(1, (int)5e7+1, i => { sum+=1.0/i;});
		WriteLine($"Using Parallel.For: {sum}");
	}
}
