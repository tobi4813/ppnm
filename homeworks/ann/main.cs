using System;
using System.IO;
using static System.Console;
using static System.Math;

static class main
{
	static void Main()
	{
		Func<double,double> g = x => Cos(5*x-1)*Exp(-x*x);
		Interpolate(g,6,12,new double[] {-1,1},50,"test1");
		Interpolate(g,6,7,new double[] {-1,1},50,"test2");
		Interpolate(g,3,12,new double[] {-1,1},50,"test3");
	}
	static void Interpolate(Func<double,double> g, int networks, int trainingPoints, double[] limits, int resolution, string name, double acc=1e-3)
	{
		
		vector xs = new vector(trainingPoints), ys = new vector(trainingPoints);
		for(int i=0;i<trainingPoints;i++) 
		{
			xs[i] = limits[0] + (limits[1] - limits[0])/(trainingPoints-1)*i; 
			ys[i] = g(xs[i]);
		}
		Directory.CreateDirectory("data");
		using (StreamWriter output = new StreamWriter($"data/{name}Training.data"))
		{
			for(int i=0;i<trainingPoints;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
		
		ANN ai = new ANN(networks);
		ai.acc = acc;
		ai.train(xs,ys);
		
		xs = new vector(resolution);
		vector interpolation = new vector(resolution), interpDerivative = new vector(resolution);
		vector interp2ndDerivative = new vector(resolution), interpIntegral = new vector(resolution);
		for(int i=0;i<resolution;i++) 
		{
			xs[i] = limits[0] + (limits[1] - limits[0])/(resolution-1)*i;
			interpolation[i] = ai.response(xs[i]);
			interpDerivative[i] = ai.Derivative(xs[i]);
			interp2ndDerivative[i] = ai.SecondDerivative(xs[i]);
			interpIntegral[i] = ai.AntiDerivative(xs[i]);
		}
		using (StreamWriter output = new StreamWriter($"data/{name}Interpolation.data"))
		{
			for(int i=0;i<resolution;i++) output.WriteLine($"{xs[i]} {interpolation[i]} {interpDerivative[i]} {interp2ndDerivative[i]} {interpIntegral[i]}");
		}
		
	}
}
