using System;
using static System.Console;
using static System.Math;
using static Integrate;

public static class main
{
	public static void Main()
	{
		WriteLine("Results below (accuracy of 0.001 fulfilled)\n");
		//WriteLine($"Integral of sqrt(x) from 0 to 1:\n Numerical: {QUAD(Test1, 0, 10000000)}\n     Exact: {2f/3}\n");
		WriteLine($"Integral of 1/sqrt(x) from 0 to 1:\n Numerical: {QUAD(Test2, 0, 1)}\n     Exact: {2}\n");
		WriteLine($"Integral of 4*sqrt(1-x²) from 0 to 1:\n Numerical: {QUAD(Test3, 0, 1)}\n     Exact: {PI}\n");
		WriteLine($"Integral of ln(x)/sqrt(x) from 0 to 1:\n Numerical: {QUAD(Test4, 0, 1)}\n     Exact: {-4}\n");
		WriteLine($"erf(1) = {Erf(1)}\nExpected: 0.84270079 (from WolframAlpha)");
		WriteLine($"erf(2) = {Erf(2)}\nExpected: 0.99532227 (from WolframAlpha)");	
	}
	static double Test1(double x) {return Sin(x);}//Sqrt(x);}
	static double Test2(double x) {return 1/Sqrt(x);}
	static double Test3(double x) {return 4*Sqrt(1-x*x);}
	static double Test4(double x) {return Log(x)/Sqrt(x);}
	//static double Erf1(double x) {return Exp(-x*x);}
	//static double Erf2(double x) {return Exp(-(z+(1-x));}
	static double Erf(double z)
	{
		if(Abs(z) <= 1)
		{ 
			Func<double,double> f = x => Exp(-x*x);
			if(z >= 0) return 2/Sqrt(PI)*QUAD(f, 0, z);
			else return 2/Sqrt(PI)*QUAD(f, 0, -z);
		}
		else
		{
			Func<double,double> f;
			if(z > 0) f = x => Exp(-Pow(z+(1-x)/x, 2))/x/x;
			else f = x => Exp(-Pow(-z+(1-x)/x, 2))/x/x;
			return 1-2/Sqrt(PI)*QUAD(f, 0, 1);			
		}
	}
}

