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
		(double[] bs, double[] cs) = Spline.Qspline(xs,ys); 
		using (StreamWriter output = new StreamWriter("qsplinepoints.data"))
		{
			for(int i=0;i<xs.Length;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
		using (StreamWriter output = new StreamWriter("qspline.data"))
		{
			int resolution = 50;

			for(int j=0;j<xs.Length-1;j++)
			{
				for(int i=0;i<resolution;i++)
				{
					double x = xs[j] + (xs[j+1]-xs[j])/(resolution-1)*i;
					double y = ys[j] + bs[j]*(x-xs[j]) + cs[j]*(x-xs[j])*(x-xs[j]);
					output.WriteLine($"{x} {y}");
				}
			}
		}
	}
	static void TrapeziumTest()
	{
		double[] xs = {1,2,3,4};
		double[] ys = {0,1,1,0};
		double area1 = Spline.LinterpIntegral(xs, ys, 2);
		double area2 = Spline.LinterpIntegral(xs, ys, 2.5);
		double area3 = Spline.LinterpIntegral(xs, ys, 3);
		double area4 = Spline.LinterpIntegral(xs, ys, 4);
		WriteLine($"Area of trapezium:\n  From 1 to 2: {area1}\n  From 1 to 2.5: {area2}\n  From 1 to 3: {area3}\n  From 1 to 4: {area4}");
		using (StreamWriter output = new StreamWriter("trapezium.data"))
		{
			for(int i=0;i<xs.Length;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
	}
	static void SinusTest()
	{
		int resolution = 10;
		double[] xs = new double[resolution];
		double[] ys = new double[resolution];
			for(int i=0;i<resolution;i++)
		{
			xs[i] = PI/(resolution-1)*i;
			ys[i] = Sin(xs[i]);
		}
		double area1 = Spline.LinterpIntegral(xs,ys,2.25);
		double area2 = Spline.LinterpIntegral(xs,ys,xs[resolution-1]);
		WriteLine($"Area of sin(x):\n  From 0 to 2.25 is ~{area1} (is actually 1.628)\n  From 0 to pi is ~{area2} (is actually 2)");
		using (StreamWriter output = new StreamWriter("sinus.data"))
		{
			for(int i=0;i<xs.Length;i++) output.WriteLine($"{xs[i]} {ys[i]}");
		}
		
	}
}
