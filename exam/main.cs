using System;
using System.IO;
using System.Diagnostics;
using static System.Math;
using static System.Console;

static class main
{
	static void Main()
	{
		random();
		longSin();
		sin();
		OperationsCount(2);
	}
	static void random()
	{
		double[] xs = {1,2,3,4,5,6,7,8};
		double[] ys = {2,4,3,1,5,7,3,1};
		Spline test = new Spline(xs, ys);
		graph(xs,ys,spline: test, filename: "random", resolution: 250, animate: true);
	}
	static void longSin()
	{
		int k = 20;
		double[] xs = new double[k];
		double[] ys = new double[k];
		for(int i=0;i<xs.Length;i++) {xs[i] = 20.0/(k-1)*i; ys[i] = Sin(xs[i]);}
		Spline test = new Spline(xs, ys);
		graph(xs,ys,spline: test, filename: "longSin", resolution: 4000);
	}
	static void sin()
	{
		int k = 20;
		double[] xs = new double[k];
		double[] ys = new double[k];
		for(int i=0;i<xs.Length;i++) {xs[i] = 6.0/(k-1)*i; ys[i] = Sin(xs[i]);}
		Spline test = new Spline(xs, ys);
		graph(xs,ys,spline: test, filename: "sin", resolution: 1000);
	}
	static void graph(double[] xs, double[] ys, Spline spline, string filename, int resolution=200, bool animate=false)
	{
		Directory.CreateDirectory("data");
		var pointsFile = new StreamWriter($"data/{filename}_points.data");
		pointsFile.WriteLine("Points");
		for(int i=0;i<xs.Length;i++) pointsFile.WriteLine($"{xs[i]} {ys[i]}");

		var interpolationFile = new StreamWriter($"data/{filename}_interp.data");
		interpolationFile.WriteLine("Points \"B_1 spline\" \"B_1 Derivative\" \"B_2 spline\"");
		for(int i=0;i<resolution-1;i++)
		{	
			double x = xs[0] + (xs[xs.Length-1] - xs[0])/(resolution) * (i+1);
			double y = spline.Berrut1(x);
			double yPrime =	spline.B1Derivative(x);
			double y2 = spline.Berrut2(x);
			interpolationFile.WriteLine($"{x} {y} {yPrime} {y2}"); 
		}
		pointsFile.Close();
		interpolationFile.Close();	
		
		if(animate)
		{	
			var animationFile = new StreamWriter($"data/{filename}_animation.data");
			for(int i=2;i<resolution;i++)
			{
				for(int j=0;j<i;j++) 
				{
					double x = xs[0] + (xs[xs.Length-1] - xs[0])/(resolution) * (j+1);
					double y = spline.Berrut1(x);
					animationFile.WriteLine($"{x} {y}");
				}
				animationFile.WriteLine("\n");
			}
			animationFile.Close();
		}
	}
	static void OperationsCount(int resolution)
	{
		double nmin = 1e6, nmax = 5e6;
		int ntestPoints = 1;
		Random rnd = new Random();
		double[] testPoints = new double[ntestPoints];
		for(int i=0;i<ntestPoints;i++) testPoints[i] = nmin + (nmax-nmin)*rnd.NextDouble();
		var opCountFile = new StreamWriter("data/operations.data");
		opCountFile.WriteLine("Points");
		for(int i=0;i<resolution;i++)
		{
			int n = (int) (nmin + (nmax-nmin)/(resolution-1) * i);
			double[] x = new double[n];
			double[] y = new double[n];
			for(int j=0;j<n;j++) { x[j] = j; y[j] = j*j; }
			Spline parabola = new Spline(x,y);

			Stopwatch stopwatch = new Stopwatch();
			double time = 0;
			for(int j=0;j<ntestPoints;j++)
			{
				stopwatch.Start();
				parabola.Berrut1(testPoints[j]);
				stopwatch.Stop();
			}
			time = stopwatch.ElapsedMilliseconds/ntestPoints;
			opCountFile.WriteLine($"{n} {time}");
		}
		opCountFile.Close();
	}
}
