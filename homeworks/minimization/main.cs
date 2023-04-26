using System;
using System.IO;
using static System.Console;
using static System.Math;
using static Minimization;

static class main
{
	static void Main()
	{	
		TestA();

		////////////// b //////////////////////////////////////////////////////////////////////
		var energy = new genlist<double>();	
		var signal = new genlist<double>();	
		var error = new genlist<double>();
		var separators = new char[] {' ', '\t'};
		var options = StringSplitOptions.RemoveEmptyEntries;
		do
		{
			string line = Console.In.ReadLine();
			if(line == null) break;
			string[] words = line.Split(separators,options);
			if(words[0]=="Energy") continue;
			energy.add(double.Parse(words[0]));
			signal.add(double.Parse(words[1]));
			error.add(double.Parse(words[2]));
		}while(true);

		Func<vector,double> Deviation = v =>
		{
			double sum=0;
			for(int i=0;i<energy.size;i++) sum+=Pow((BW(energy[i],v)-signal[i])/error[i], 2);
			return sum; 	
		};
		vector guess = new vector(125,6,30);
		vector higgs = qnewton(Deviation, guess);
		int resolution = 1000; 
		using (StreamWriter output = new StreamWriter("fit.data"))
		{
			output.WriteLine($"Energy \" m={Round(higgs[0],2)}GeV/c²\"");
			for(int i=0;i<resolution;i++)
			{	
				double E = energy[0] + (energy[energy.size-1] - energy[0])/resolution*(i+1);
				output.WriteLine($"{E} {BW(E,higgs)}");
			}
		}
		////////////// c /////////////////////////////////////////////////////////////
		vector higgsAmoeba = amoeba(Deviation, guess).Item1;
		using (StreamWriter output = new StreamWriter("amoebafit.data"))
		{
			output.WriteLine($"Energy \" m={Round(higgsAmoeba[0],2)}GeV/c²\"");
			for(int i=0;i<resolution;i++)
			{	
				double E = energy[0] + (energy[energy.size-1] - energy[0])/resolution*(i+1);
				output.WriteLine($"{E} {BW(E,higgsAmoeba)}");
			}
		}
	}
	static double BW(double E, vector v)
	{
		double m=v[0],gamma=v[1],A=v[2];
		return A/(Pow(E-m,2)+gamma*gamma/4);
	}
	static void TestA()
	{
		vector x0 = new vector(100,-2000);
		(vector rb,int steps) = qnewtonCount(Rosenbrock, x0, acc:1e-8);
		Divide();
		WriteLine($"Rosenbrock's valley function has a minimum at (x,y) = ({rb[0]},{rb[1]})");
		x0.print($"Computed in {steps} steps from the initial guess at"); 

		(vector ab, int absteps) = amoeba(Rosenbrock, x0, acc: 1e-7, initialSize:20);
		WriteLine($"Using the downhill simplex method: (x,y) = ({ab[0]},{ab[1]}) in {absteps} steps");
	
		Divide();
		WriteLine("According to WikiPedia, Himmelblau's function has 4 minima. They match the 4 numerical ones:");
		matrix x0s = new matrix("4 3;-1 1;-4 -4; 3 -2");
		for(int i=0;i<4;i++)
		{
			x0 = new vector(x0s[i,0], x0s[i,1]);
			(vector hb,int hbsteps) = qnewtonCount(Himmelblau, x0);
			WriteLine($"(x,y) = ({hb[0]},{hb[1]}) computed in {hbsteps} steps from the initial point ({x0[0]},{x0[1]})");
		}
	
	}
	
	static double Rosenbrock(vector v) {double x=v[0], y=v[1]; return Pow((1-x),2)+100*Pow((y-x),2);}
	static double Himmelblau(vector v) {double x=v[0], y=v[1]; return Pow((x*x+y-11),2)+Pow((x+y*y-7),2);}
	static void Divide(int n=100) {WriteLine(new string('-', n));}
}
