using System;
using static System.Math;

public static class MC
{
	public static (double,double) PlainMC(Func<vector,double> f, vector a, vector b, int N)
	{
		int dim = a.size;
		double V = 1;
		for(int i=0;i<dim;i++) V *= b[i] - a[i];
		double sum = 0, sum2 = 0;
		vector x = new vector(dim);
		Random rnd = new Random();
		for(int i=0;i<N;i++)
		{
			for(int k=0;k<dim;k++) x[k] = a[k] + rnd.NextDouble()*(b[k] - a[k]);
			double fx = f(x);
			sum += fx;
			sum2 += fx*fx;
		}
		double mean = sum/N;
		double sigma = Sqrt(sum2/N - mean*mean);
		return (mean*V, sigma*V/Sqrt(N));
	}
	public static (double,double) QuasiMC(Func<vector,double> f, vector a, vector b, int N, double[] xs=null, double[] ys=null, int offset=0)
	{
		int dim = a.size;
		double V = 1;
		for(int i=0;i<dim;i++) V *= b[i] - a[i];
		double sum = 0, sum2 = 0;
		vector x = new vector(dim);
		vector x2 = new vector(dim);
		for(int n=0;n<N;n++)
		{
			for(int k=0;k<dim;k++) 
			{
				x[k] = a[k] + Corput(n,Prime(k+offset))*(b[k] - a[k]);
				x2[k] = a[k] + Corput(n,Prime(k+7+offset))*(b[k] - a[k]);
			}
			if(xs != null) {xs[n] = x[0]; ys[n] = x[1];} 
			sum += f(x);
			sum2 += f(x2);
		}
		double mean = sum/N;
		double mean2 = sum2/N;
		double err = V*Abs(mean-mean2);
		return (mean*V, err);
	}
	static double Corput(int n, int b)
	{
		double q = 0, bk = (double)1/b;
		while(n > 0) {q += (n % b)*bk; n /= b; bk /= b;}
		return q;
	}
	static void Halton(int n, int d, vector x)
	{
		int[] base_ = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		int maxd = base_.Length/sizeof(int);
		if(d > maxd) for(int i=0;i<d;i++) x[i] = Corput(n,base_[i]);
	}	
	static int Prime(int i)
	{
		int[] primes = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		if(i >= primes.Length) return Prime(i-primes.Length);
		else return primes[i];
	}
}
