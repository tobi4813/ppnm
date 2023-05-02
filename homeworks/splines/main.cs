using System;
using System.IO;
using static System.Console;
using static System.Math;

static class main
{
	static void Main()
	{
		TrapeziumTest();
		SinusTest();
		double[] xs = {1,2,3,4,5,6,7,8,9,10};
		double[] ys = {1,4,2,6,12,2,9,7,10,5};
		QsplineTest(xs,ys,"Random");
		for(int i=0;i<10;i++) {xs[i] = 2*PI/9*i; ys[i] = Sin(xs[i]);}
		QsplineTest(xs,ys,"Sine", areas: new double[] {2.25, PI, 2*PI, 1.628, 2, 0});

	}
	static void QsplineTest(double[] xs, double[] ys, string name, int resolution=50, double[] areas = null)
	{
		Spline qSpline = new Spline(xs,ys);
		qSpline.Qspline();
		if(areas!=null) 
		{
			WriteLine($"\nArea of {name} beneath a quadratic spine:");
			for(int i=0;i<areas.Length/2;i++)
				{
					qSpline.Qintegral(areas[i]);
					double area = qSpline.A;
					WriteLine($"  From 0 to {areas[i]}: {area} (should be {areas[i+areas.Length/2]})");
				}
		}
		Directory.CreateDirectory("data");
		using (StreamWriter output = new StreamWriter($"data/q{name}points.data"))
		{
			for(int i=0;i<xs.Length;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
		using (StreamWriter output = new StreamWriter($"data/q{name}.data"))
		{
			(genlist<double> x,genlist<double> y) = qSpline.Qgraph(resolution);
			for(int i=0;i<x.size;i++) output.WriteLine($"{x[i]} {y[i]}");
		}
		using (StreamWriter output = new StreamWriter($"data/q{name}Derivative.data"))
		{
			(genlist<double> x,genlist<double> y) = qSpline.Qderivative(resolution);
			for(int i=0;i<x.size;i++) output.WriteLine($"{x[i]} {y[i]}");		
		}

	}
	static void TrapeziumTest()
	{
		Directory.CreateDirectory("data");
		double[] xs = {1,2,3,4};
		double[] ys = {0,1,1,0};
		Spline trapezium = new Spline(xs,ys);
		trapezium.LinterpIntegral(2);
		double area1 = trapezium.A;
		trapezium.LinterpIntegral(2.5);
		double area2 = trapezium.A;
		trapezium.LinterpIntegral(3);
		double area3 = trapezium.A;
		trapezium.LinterpIntegral(4);
		double area4 = trapezium.A;
		WriteLine($"Area of trapezium:\n  From 1 to 2:   {area1}\n  From 1 to 2.5: {area2}\n  From 1 to 3:   {area3}\n  From 1 to 4:   {area4}");
		using (StreamWriter output = new StreamWriter("data/trapezium.data"))
		{
			for(int i=0;i<xs.Length;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
	}
	static void SinusTest()
	{
		Directory.CreateDirectory("data");
		int resolution = 10;
		double[] xs = new double[resolution];
		double[] ys = new double[resolution];
			for(int i=0;i<resolution;i++)
		{
			xs[i] = PI/(resolution-1)*i;
			ys[i] = Sin(xs[i]);
		}
		Spline sinus = new Spline(xs,ys);
		sinus.LinterpIntegral(2.25);
		double area1 = sinus.A;
		sinus.LinterpIntegral(xs[resolution-1]);
		double area2 = sinus.A;
		WriteLine($"Area of sin(x):\n  From 0 to 2.25 is ~{area1} (is actually 1.628)\n  From 0 to pi is ~{area2} (is actually 2)");
		using (StreamWriter output = new StreamWriter("data/sinus.data"))
		{
			for(int i=0;i<xs.Length;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
		
	}
}
