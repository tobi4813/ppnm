using System;
using static System.Math;
using static System.Double;
using System.IO;

public static class Integrate
{
	public static double OpenAd(Func<double,double> f, double a, double b, double acc=1e-4, double eps=1e-4, double f2=NaN, double f3=NaN)
	{
		double h=b-a;
		if(IsNaN(f2)){f2 = f(a + 2*h/6); f3 = f(a + 4*h/6);}
		double f1 = f(a + h/6), f4 = f(a + 5*h/6);
		double Q = (2*f1 + f2 + f3 + 2*f4)/6*h;
		double q = (  f1 + f2 + f3 +   f4)/4*h;
		double err = Abs(Q-q);
		if(err <= acc+eps*Abs(Q)) return Q;
		else return OpenAd(f, a, (a+b)/2, acc/Sqrt(2), eps, f1, f2) + OpenAd(f, (a+b)/2, b, acc/Sqrt(2), eps, f3, f4);
	}
	public static (double,int) OpenAdCount(Func<double,double> f, double a, double b, 
											double acc=1e-4, double eps=1e-4, double f2=NaN, double f3=NaN, int count=0)
	{
		/*using (StreamWriter output = new StreamWriter("count.txt", append: true))
		{
			output.WriteLine(1);
		}*/
		double h=b-a;
		if(IsNaN(f2)){f2 = f(a + 2*h/6); f3 = f(a + 4*h/6); count+=2;}
		double f1 = f(a + h/6), f4 = f(a + 5*h/6); count+=2;
		double Q = (2*f1 + f2 + f3 + 2*f4)/6*h;
		double q = (  f1 + f2 + f3 +   f4)/4*h;
		double err = Abs(Q-q);
		if(err <= acc+eps*Abs(Q)) return (Q, count); 
		else 
		{
			(double left, int leftCount) = OpenAdCount(f, a, (a+b)/2, acc/Sqrt(2), eps, f1, f2);
			(double right, int rightCount) = OpenAdCount(f, (a+b)/2, b, acc/Sqrt(2), eps, f3, f4);
			return (left+right, leftCount+rightCount+count); // Without the +count, only the last branches in the recursion tree are counted
		}			
	}
	public static double OpenAdCC(Func<double,double> f, double a, double b, double acc=1e-4, double eps=1e-4)
	{
		Func<double,double> ClenshawCurtis = theta => f((a+b)/2 + (b-a)/2*Cos(theta))*Sin(theta) * (b-a)/2; 
		return OpenAd(ClenshawCurtis, 0, PI, acc, eps);
	}

	public static (double, int) OpenAdCCCount(Func<double,double> f, double a, double b, double acc=1e-4, double eps=1e-4)
	{
		Func<double,double> ClenshawCurtis = theta => f((a+b)/2 + (b-a)/2*Cos(theta))*Sin(theta) * (b-a)/2; 
		return OpenAdCount(ClenshawCurtis, 0, PI, acc, eps);
	}
}
