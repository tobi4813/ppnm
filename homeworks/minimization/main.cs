using System;
using static System.Console;
using static System.Math;
using static Minimization;

static class main
{
	static void Main()
	{
		vector x0 = new vector(5,12);
		(vector rb,int steps) = qnewtonCount(Rosenbrock, x0);
		Divide();
		WriteLine($"Rosenbrock's valley function has a minimum at (x,y) = ({rb[0]},{rb[1]})");
		x0.print($"Computed in {steps} steps from the initial guess of"); 
		
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
