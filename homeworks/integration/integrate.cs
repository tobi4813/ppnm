using System;
using static System.Math;
using static System.Double;

public static class Integrate
{
	public static double QUAD(Func<double,double> f, double a, double b, double acc=1e-3, double eps=1e-3, double f2=NaN, double f3=NaN)
	{
		double h=b-a;
		if(IsNaN(f2)){f2 = f(a + 2*h/6); f3 = f(a + 4*h/6);}
		double f1 = f(a + h/6), f4 = f(a + 5*h/6);
		double Q = (2*f1 + f2 + f3 + 2*f4)/6*h;
		double q = (  f1 + f2 + f3 +   f4)/4*h;
		double err = Abs(Q-q);
		if(err <= acc+eps*Abs(Q)) return Q;
		else return QUAD(f, a, (a+b)/2, acc/Sqrt(2), eps, f1, f2) + QUAD(f, (a+b)/2, b, acc/Sqrt(2), eps, f3, f4);
	}

	
}
