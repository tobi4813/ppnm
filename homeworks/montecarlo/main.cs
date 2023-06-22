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
		Stratified();
	}
	static void Stratified()
	{
		vector a = new vector(-1,-1);
		vector b = new vector(1,1);
		Func<vector,double> fun = x => 
		{
			if(x.norm() <= 1) return 1;
			return 0;
		};
		genlist<double> xs = new genlist<double>();
		genlist<double> ys = new genlist<double>();
		//MC.PlainMC(fun,a,b,10000,xs,ys);

		int N = 8000;
		(double integral, double err) = MC.StratifiedMC(fun, a, b ,N , nmin: 10/*(int) (N/200)*/, xs: xs, ys: ys);
		//WriteLine($"mc:     {integral}");
		//WriteLine($"Actual: {PI/4*0.8*0.8}");
		//WriteLine(err);
		//WriteLine(xs.size);
		N = xs.size;
		using (var output = new StreamWriter("data/samples_stratified.data"))
		{
			output.WriteLine($"\"Area estimated as {Math.Round(integral,4)}±{Math.Round(err,4)}\"");
			if(xs.size > 0) for(int i=0;i<xs.size;i++) output.WriteLine($"{xs[i]} {ys[i]}");
			else output.WriteLine("0 0");
		}

		
		xs = new genlist<double>();
		ys = new genlist<double>();
		(integral, err) = MC.PlainMC(fun,a,b,N,xs,ys);
		using (var output = new StreamWriter("data/samples_plain.data"))
		{
			output.WriteLine($"\"Area estimated as {Math.Round(integral,4)}±{Math.Round(err,4)}\"");
			if(xs.size > 0) for(int i=0;i<xs.size;i++) output.WriteLine($"{xs[i]} {ys[i]}");
			else output.WriteLine("0 0");
			
		}


		xs = new genlist<double>();
		ys = new genlist<double>();
		(integral, err) = MC.QuasiMC(fun,a,b,N,xs,ys);		
		using (var output = new StreamWriter("data/samples_quasi.data"))
		{
			output.WriteLine($"\"Area estimated as {Math.Round(integral,4)}±{Math.Round(err,4)}\"");
			if(xs.size > 0) for(int i=0;i<xs.size;i++) output.WriteLine($"{xs[i]} {ys[i]}");
			else output.WriteLine("0 0");
		}

		
		using (var output = new StreamWriter("data/circle.data")){
			for(int i=0;i<1000;i++)
			{
				double x = a[0] + (b[0] - a[0])/999*i;
				double y = Sqrt(1-x*x);
				output.WriteLine($"{x} {y}");
			}	
			for(int i=0;i<1000;i++)
			{
				double x = a[0] + (b[0] - a[0])/999*i;
				double y = -Sqrt(1-x*x);
				output.WriteLine($"{-x} {y}");
			}
		}
	}
	static void Wtf() //What the func?
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
		WriteLine($"  {N} points used\n");
		
	}
	static void Sphere()
	{
		vector a = new vector(-2,-2,-2);
		vector b = new vector(2,2,2);
		Func<vector,double> Spherical = x => {if(x.norm()>2) return 0; return 1;}; 
		int N = 100000;
		(double integral,double err) = MC.PlainMC(Spherical,a,b,N);
		WriteLine($"Volume of sphere with radius 2:");
		WriteLine($" monte carlo {N} points: {integral}±{err}");
		WriteLine($"                     Exact: {4*PI*8/3}");
		(integral,err) = MC.QuasiMC(Spherical,a,b,N);
		WriteLine($"         quasi monte carlo: {integral}±{err}");
	}
	static void Unitcircle()
	{
		vector a = new vector(-1,-1);
		vector b = new vector(1,1);
		Func<vector,double> UnitCircle = x => {if(x.norm()>1) return 0; return 1;};
		int resolution = 60;//0;
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
				(double qIntegral, double qError) = MC.QuasiMC(UnitCircle,a,b,N);
				realError[i] = Abs(PI - integral);
				output.WriteLine($"{n} {N} {integral} {error} {realError[i]} {qIntegral} {qError}"); 
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

