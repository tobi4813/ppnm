using System;
using System.IO;
using static System.Math;
using static System.Console;
using static Minimization;

static class main
{
	static void Main()
	{
		Unitcircle();
		Wtf();
		Sphere();
		
	}
	static void Wtf()
	{
		vector a = new vector(0,0,0);
		vector b = new vector(PI,PI,PI);
		Func<vector,double> fun = u => 
		{
			double x = u[0], y = u[1], z = u[2];
			double cube = Cos(x)*Cos(y)*Cos(z);
			return 1/(1-cube)/(PI*PI*PI);
		};
		int N = 1000000;
		(double integral, double err) = MC.PlainMC(fun,a,b,N);
		WriteLine($"∫0π  dx/π ∫0π  dy/π ∫0π  dz/π [1-cos(x)cos(y)cos(z)]⁻¹ = {integral}");
		WriteLine($"  approximate error is {err}"); 
		WriteLine($"  {N} points used");
		
	}
	static void Sphere()
	{
		vector a = new vector(0,0,0);
		vector b = new vector(2,PI,2*PI);
		Func<vector,double> Spherical = x => x[0]*x[0]*Sin(x[1]);
		int N = 100000;
		(double integral,double err) = MC.PlainMC(Spherical,a,b,N);
		WriteLine($"Volume of sphere with radius 2:");
		WriteLine($" monte carlo {N} points: {integral}±{err}");
		WriteLine($"                     Exact: {4*PI*8/3}");
	}
	static void Unitcircle()
	{
		vector a = new vector(0,0);
		vector b = new vector(1,2*PI);
		Func<vector,double> UnitCircle = x => x[0]; // area of circle is integral of r dr d(theta), so the function is just r (= x[0])
		int resolution = 60;
		double  min = 1;
		double  max = 1e5;
		Directory.CreateDirectory("data");
		double[] realError = new double[resolution];
		int[] Ns = new int[resolution];
		using (StreamWriter output = new StreamWriter("data/uc.data"))
		{
			for(int i=0;i<resolution;i++)
			{
				double n = Log10(min) + (Log10(max)-Log10(min))/(resolution-1)*i;
				int N = (int)Pow(10,n);
				Ns[i] = N;
				(double integral, double error) = MC.PlainMC(UnitCircle,a,b,N);
				realError[i] = Abs(PI - integral);
				output.WriteLine($"{n} {N} {integral} {error} {realError[i]}"); 
			}
		}	
		Func<vector,int,double> FitFunc = (scale,N) => scale[0]/Sqrt(N);
		Func<vector,double> Deviation = scale => 
		{
			double sum = 0;
			for(int i=0;i<resolution;i++) sum+=Pow((FitFunc(scale,Ns[i])-realError[i])/1,2); 
			return sum;
		};
		vector guess = new vector(1.0);
		vector fit = amoeba(Deviation, guess).Item1;
		using (StreamWriter output = new StreamWriter("data/ucFit.data"))
		{
			for(int i=0;i<resolution;i++) output.WriteLine($"{Log10(Ns[i])} {Ns[i]} {FitFunc(fit,Ns[i])}");
		}
	}
}

