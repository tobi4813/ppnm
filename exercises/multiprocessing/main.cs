using System;
using System.Threading; 
using static System.Console;
public static class main
{
	public static void Main(string[] args)
	{
		int nthreads = 1;
		int nterms = (int)1e8;
		double sum = 0;
		foreach(var arg in args)
		{
			var words = arg.Split(":");
			if (words[0] == "-threads") nthreads = int.Parse(words[1]);
			else if (words[0] == "-terms") nterms = (int)float.Parse(words[1]);
		}
		data[] x = new data[nthreads];
		for (int i=0; i<nthreads; i++)
		{
			x[i] = new data();
			x[i].a = 1 + nterms/nthreads*i;
			x[i].b = 1 + nterms/nthreads*(i+1);
		}
		x[x.Length-1].b=nterms+1;

		var threads = new Thread[nthreads];
		for (int i=0; i<nthreads; i++)
		{
			threads[i] = new Thread(harmonic_sum);
			threads[i].Start(x[i]);
		}

		for (int i=0; i<nthreads; i++) threads[i].Join();

		for (int i=0; i<nthreads; i++) sum += x[i].sum;

		WriteLine(sum);
	}
	
	public static void harmonic_sum(object obj)
	{
		var local = (data)obj;
		local.sum = 0;
		for (int i = local.a; i<local.b; i++) 
		local.sum += 1.0/i; 
	}
}


public class data 
{
	public int a,b;
	public double sum;
}

