using System;
using System.IO;
using static System.Math;

public partial class Spline
{
	public double[] x,y;
	int n;
	
	public Spline(double[] x,double [] y)
	{
		this.x = x;
		this.y = y;
		this.n = x.Length;
	}
	public double Berrut1(double z)
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
	public double B1Derivative(double z)
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
	public double Berrut2(double z)
	{
		double numerator =0, denominator = 0;
		int k = 1;
		for(int i=0;i<n;i++)
		{
			double diff = z-x[i];
			numerator += k * Pow(-1, i) * y[i]/diff;
			denominator += k * Pow(-1, i)/diff;
			k = 2;
			if(i == n-2) k = 1;
		}
		return numerator/denominator;
	}
	public double B2Derivative(double z)
	{
		double numerator = 0, denominator = 0, numeratorPrime = 0, denominatorPrime = 0;
		int k = 1;
		for(int i=0;i<n;i++)
		{
			double diff = z-x[i];
			double numerator_i = k*Pow(-1, i) * y[i]/diff;
			double denominator_i = k*Pow(-1, i)/diff;
			numerator += numerator_i;
			numeratorPrime -= numerator_i/diff;
			denominator += denominator_i;
			denominatorPrime -= denominator_i/diff;
			k = 2;
			if(i == n-2) k = 1;
		}
		denominatorPrime /= (-denominator*denominator); 
		return numeratorPrime/denominator + numerator*denominatorPrime;
	}
}
