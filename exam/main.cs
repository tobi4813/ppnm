using System;
using System.IO;
using static System.Math;
using static System.Console;

static class main
{
	static void Main()
	{
		//double[] xs = {1,2,3,4,5,6,7,8};
		int k = 250;
		double[] xs = new double[k];
		//double[] ys = {2,4,3,1,5,7,3,1};
		double[] ys = new double[k];

		for(int i=0;i<xs.Length;i++) {xs[i] = 6*PI/(k-1)*i; ys[i] = Sin(xs[i]);}
		Spline test = new Spline(xs, ys);
		graph(xs,ys,spline: test, filename: "test", resolution: 10000);
	}
	static void random()
	{
		
	}
	static void longSin()
	{

	}
	static void shortSin()
	{

	}
	static void graph(double[] xs, double[] ys, Spline spline, string filename, int resolution=200)
	{
		Directory.CreateDirectory("data");
		var pointsFile = new StreamWriter($"data/{filename}_points.data");
		for(int i=0;i<xs.Length;i++) pointsFile.WriteLine($"{xs[i]} {ys[i]}");

		var interpolationFile = new StreamWriter($"data/{filename}_interp.data");
		//using (var output = new StreamWriter("interpolation.data"))
		//{
			for(int i=0;i<resolution;i++)
			{	
				double x = xs[0] + (xs[xs.Length-1] - xs[0])/(resolution-1) * i;
				double y = spline.Berrut(x);
				double yPrime =	spline.BDerivative(x);
				interpolationFile.WriteLine($"{x} {y} {yPrime}"); 
			}
		pointsFile.Close();
		interpolationFile.Close();
		
		
	}
}
