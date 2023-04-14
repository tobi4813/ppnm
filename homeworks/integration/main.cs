using System;
using System.IO;
using static System.Console;
using static System.Math;
using static Integrate;

public static class main
{
	public static void Main()
	{
		Test();
	}
	static void Test()
	{
		WriteLine("\n------< A) >---------------------------------------------------------------------\n");
		WriteLine("Results below (accuracy of 0.001 fulfilled)\n");
		WriteLine($"Integral of sqrt(x) from 0 to 1:\n Numerical: {OpenAd(Test1, 0, 1)}\n     Exact: {2f/3}\n");
		WriteLine($"Integral of 1/sqrt(x) from 0 to 1:\n Numerical: {OpenAd(Test2, 0, 1)}\n     Exact: {2}\n");
		WriteLine($"Integral of 4*sqrt(1-x²) from 0 to 1:\n Numerical: {OpenAd(Test3, 0, 1)}\n     Exact: {PI}\n");
		WriteLine($"Integral of ln(x)/sqrt(x) from 0 to 1:\n Numerical: {OpenAd(Test4, 0, 1)}\n     Exact: {-4}\n");

		WriteLine("erf(1):");
		WriteLine($"        With integrator: {Erf(1)}");
		WriteLine($"               Expected: 0.84270079 (from WolframAlpha)");
		WriteLine($"     From plot exercise: {erf(1)}");
		WriteLine($"Integrator (better acc): {Erf(1,1e-7,1e-7)}");

		WriteLine("erf(2):");
		WriteLine($"        With integrator: {Erf(2)}");
		WriteLine($"               Expected: 0.99532227 (from WolframAlpha)");
		WriteLine($"     From plot exercise: {erf(2)}");
		WriteLine($"Integrator (better acc): {Erf(2,1e-7,1e-7)}");

		WriteLine("\n------< B) >---------------------------------------------------------------------\n");

		string[] pythonCounts = File.ReadAllText("pythonCounts.txt").Split("\n");
		//string test2Count = pythonCounts[0].Split(" ")[1];
		//string test4Count = pythonCounts[1].SPlit(" ")[1];
		
		Write("Integral of 1/sqrt(x) from 0 to 1 (value, # of integrand evaluations)");
		WriteLine(" with abs/rel acc=1e-4 (scipy.quad still achieves a much better accuracy somehow)"); 
		WriteLine($"   Open Adaptive: {OpenAdCount(Test2,0,1, 1e-4, 1e-4)}");
		WriteLine($" Clenshaw-Curtis: {OpenAdCCCount(Test2,0,1, 1e-4, 1e-4)}");
		WriteLine($"      scipy.quad: {pythonCounts[0]}");

		WriteLine("\nIntegral of ln(x)/sqrt(x) from 0 to 1 (value, # of integrand evaluations):");
		WriteLine($"   Open Adaptive: {OpenAdCount(Test4,0,1,1e-4,1e-4)}");
		WriteLine($" Clenshaw-Curtis: {OpenAdCCCount(Test4,0,1,1e-4,1e-4)}"); 
		WriteLine($"      scipy.quad: {pythonCounts[1]}");

	}
	static double Test1(double x) {return Sin(x);}//Sqrt(x);}
	static double Test2(double x) {return 1/Sqrt(x);}
	static double Test3(double x) {return 4*Sqrt(1-x*x);}
	static double Test4(double x) {return Log(x)/Sqrt(x);}
	//static double Erf1(double x) {return Exp(-x*x);}
	//static double Erf2(double x) {return Exp(-(z+(1-x));}
	static double Erf(double z, double acc=1e-3, double eps=1e-3)
	{
		if(Abs(z) <= 1)
		{ 
			Func<double,double> f = x => Exp(-x*x);
			if(z >= 0) return 2/Sqrt(PI)*OpenAd(f, 0, z, acc, eps);
			else return 2/Sqrt(PI)*OpenAd(f, 0, -z, acc, eps);
		}
		else
		{
			Func<double,double> f;
			if(z > 0) f = x => Exp(-Pow(z+(1-x)/x, 2))/x/x;
			else f = x => Exp(-Pow(-z+(1-x)/x, 2))/x/x;
			return 1-2/Sqrt(PI)*OpenAd(f, 0, 1, acc, eps);			
		}
	}
	static double erf(double z)
	{
		if(z<0) return -erf(-z);
		double [] a = {0.254829592, -0.284496736, 1.421413741, -1.453152027, 1.061405429};
		double t = 1/(1+0.3275911*z);
		double sum = t*(a[0] + t*(a[1] + t*(a[2] + t*(a[3] + t*a[4]))));
		return 1-sum*Exp(-z*z);
		
	}
}

