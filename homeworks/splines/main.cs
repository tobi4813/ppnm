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
		QsplineTest(xs,ys,"Random", testCoeff: true);
		for(int i=0;i<10;i++) {xs[i] = 2*PI/9*i; ys[i] = Sin(xs[i]);}
		QsplineTest(xs,ys,"Sine", areas: new double[] {2.25, PI, 2*PI, 1.628, 2, 0});

	}
	static void QsplineTest(double[] xs, double[] ys, string name, int resolution=50, double[] areas = null, bool testCoeff=false)
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
		using (StreamWriter output = new StreamWriter($"data/q{name}Antiderivative.data"))
		{
			(genlist<double> x,genlist<double> y) = qSpline.Qantiderivative(resolution, offset: -1);
			for(int i=0;i<x.size;i++) output.WriteLine($"{x[i]} {y[i]}");		
		}

		if(testCoeff)
		{
			WriteLine("\nVeriying coefficients:");
			xs = new double[] {1,2,3,4,5};
			ys = new double[] {1,1,1,1,1};
			Verify(xs, ys);
			ys = new double[] {1,2,3,4,5};
			Verify(xs, ys);
			ys = new double[] {1,4,9,16,25};
			Verify(xs, ys);
		}
	}
	static void Verify(double[] xs, double[] ys)
	{
		WriteLine($" For x: {xs[0]}, {xs[1]}, {xs[2]}, {xs[3]}, {xs[4]}\n     y: {ys[0]}, {ys[1]}, {ys[2]}, {ys[3]}, {ys[4]}");
		int n = xs.Length - 1;
		vector dx = new vector(n);
		vector p = new vector(n);
		vector c1 = new vector(n);
		vector c2 = new vector(n);
		vector b = new vector(n);
		for(int i=0;i<n;i++)
		{
			double dxi = xs[i+1] - xs[i];
			double dyi = ys[i+1] - ys[i]; 
			dx[i] = dxi; 
			p[i] = dyi/dxi;
		}
		c1[0] = 0;
		c1[1] = 1/dx[1] * (p[1] - p[0] - c1[0]*dx[0]);
		c1[2] = 1/dx[2] * (p[2] - p[1] - c1[1]*dx[1]);
		c1[3] = 1/dx[3] * (p[3] - p[2] - c1[2]*dx[2]);

		c2[3] = 0;
		c2[2] = 1/dx[2] * (p[3] - p[2] - c2[3]*dx[3]);
		c2[1] = 1/dx[1] * (p[2] - p[1] - c2[2]*dx[2]);
		c2[0] = 1/dx[0] * (p[1] - p[0] - c2[1]*dx[1]);

		c1 += c2;
		c1 /= 2;

		for(int i=0;i<n;i++) b[i] = p[i] - c1[i] * dx[i];

		Spline qSpline = new Spline(xs,ys);
		qSpline.Qspline();
		WriteLine("  B coefficients:");
		string auto = "   By program: ";
		string manual = "        By me: ";
		for(int i=0;i<qSpline.b.Length;i++)
		{
			auto += $"{qSpline.b[i]} ";
			manual += $"{b[i]} ";
		}
		WriteLine(auto);
		WriteLine(manual);
		
		WriteLine("  C coefficients:");
		auto = "   By program: ";
		manual = "        By me: ";
		
		for(int i=0;i<qSpline.c.Length;i++)
		{
			auto += $"{qSpline.c[i]} ";
			manual += $"{c1[i]} ";
		}
		WriteLine(auto);
		WriteLine(manual);
		WriteLine("");
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
