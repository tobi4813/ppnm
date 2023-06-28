using System;
using System.IO;
using static System.Math;

public partial class Spline
{
	public double[] x,y,b,c,d;
	public double A;
	int n;
	
	public Spline(double[] x,double [] y)
	{
		this.x = x;
		this.y = y;
		this.n = x.Length;
	}
	public double Berrut(double z)
	{
		double numerator = 0, denominator = 0;
		for(int i=0;i<n;i++)
		{
			double diff = z-x[i];
			numerator += Pow(-1, i) * y[i]/diff;
			denominator += Pow(-1, i)/diff;
		}
		return numerator/denominator;
	}
	public double BDerivative(double z)
	{
		double numerator = 0, denominator = 0, numeratorPrime = 0, denominatorPrime = 0;
		for(int i=0;i<n;i++)
		{
			double diff = z-x[i];
			double numerator_i = Pow(-1, i) * y[i]/diff;
			double denominator_i = Pow(-1, i)/diff;
			numerator += numerator_i;
			numeratorPrime -= numerator_i/diff;
			denominator += denominator_i;
			denominatorPrime -= denominator_i/diff;
		}
		denominatorPrime /= (-denominator*denominator); // numerator = a, denominator = b: B1(x) = a/b, B1'(x) = a'/b + a*(1/b)', (1/b)' = d(1/b)/dx = d(1/b)/db * db/dx = (-1/b²)b'
		return numeratorPrime/denominator + numerator*denominatorPrime;
	}
	/*public double BAntiDerivative(double z)
	{
		int i = 0;
	}
	int Binsearch(double z)
	{
		int i = 0; j = n;
		while(j-i > 1)
		{
			int mid = (i+j)/2;
			if(z > x[mid]) i = mid; else j = mid;
		}
		return i;
	}*/

	
	(genlist<double>,genlist<double>) Graph(Func<int,double,double> S, int resolution) // S(i,x) = ...
	{
		genlist<double> xs = new genlist<double>();
		genlist<double> ys = new genlist<double>();
		for(int i=0;i<x.Length-1;i++)
			for(int j=0;j<resolution;j++)
			{
				double x = this.x[i] + (this.x[i+1] - this.x[i])/(resolution-1)*j;
				xs.add(x);
				ys.add(S(i,x));
			}
		return (xs,ys);
	}
}
