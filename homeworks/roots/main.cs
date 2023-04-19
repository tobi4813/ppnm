using System;
using static System.Console;
using static Roots;
using static System.Math;
using static solveODE;

static class main
{
	static void Main()
	{
		Tests(); //A
		WriteLine("Fix Dx going to 0");
		double rmin = 0.1, rmax=20;
		double f0 = rmin-rmin*rmin;
		//drive(Hydrogen, rmin, f0, rmax, acc: 1e-7, eps: 1e-7);   
		
		
	}
	static Func<vector,vector> Parabola(double a, double b, double c)
	{
		Func<vector,vector> f = x => 
		{
			vector y = new vector(x.size);
			for(int i=0;i<y.size;i++) y[i] = a*x[i]*x[i] + b*x[i] + c;
			return y;
		};
		return f;
	}
	static Func<vector,vector> Parabola(double[] a, double[] b, double[] c)
	{
		Func<vector,vector> f = x => 
		{
			vector y = new vector(x.size);
			for(int i=0;i<y.size;i++) y[i] = a[i]*x[i]*x[i] + b[i]*x[i] + c[i];
			return y;
		};
		return f;
	}
	static vector Test(vector v)
	{
		double x = v[0], y = v[1], z = v[2];
		double eq1 =  2*x + 2*y +   z - 20;
		double eq2 = -3*x -   y-    z + 18;
		double eq3 =    x +   y + 2*z - 16;
		return new vector(eq1, eq2, eq3);
	}
	static vector Rosenbrock(vector x)
	{ 
		double dfdx = -2*(1-x[0])-200*x[0]*(x[1]-x[0]*x[0]);
		double dfdy = 200*(x[1]-x[0]*x[0]);
		return new vector(dfdx, dfdy);
	}
	static vector Hydrogen(double r, vector y)
	{	
		double E = 1;
		double f = y[0];
		double fP = y[1];
		double fPP = -2*E*f-(2/r)*f; //f''
		return new vector(fP, fPP);
	}	
	static void Tests()
	{
		
		double eps = 1e-3;
		vector x0 = new vector(1.0);
		vector root = Newton(Parabola(1,0,0), x0, eps);
		PrintResult("f(x) = x² is", Parabola(1,0,0), root, x0, eps);

		root = Newton(Parabola(1,0,1), x0, eps);
		PrintResult("f(x) = x²+1 is", Parabola(1,0,1), root, x0, eps);	eps = 1e-3;

		root = Newton(Parabola(4,-2,-6), x0, eps);
		PrintResult("f(x) = 4x²-2x-6 is", Parabola(4,-2,-6), root, x0, eps);

		x0 = new vector(-1.5);
		root = Newton(Parabola(4,-2,-6), x0, eps);
		PrintResult("f(x) = 4x²-2x-6 is also", Parabola(4,-2,-6), root, x0, eps);

		x0 = new vector(1,1);
		root = Newton(Parabola(new double[] {3,-4}, new double[] {0,1}, new double[] {0,0}), x0, eps);
		PrintResult("f(x) = (3x²,4y²+y) is", Parabola(new double[] {3,-4}, new double[] {0,1}, new double[] {0,0}), root, x0, eps);

		x0 = new vector(1,1,1);
		root = Newton(Test, x0, eps);
		WriteLine("\nSolving 3 equations with 3 unknows using our linear equation algorithm:");
		WriteLine("2x+2y+z=20\n-3x-y-z=-18\nx+y+2z=16");
		matrix eqs = new matrix("2,2,1;-3,-1,-1;1,1,2");
		vector b = new vector(20,-18,16);
		vector x = new QRGS(eqs).solve(b);
		x.print("x,y,z =");
	
		PrintResult("f(x,y,z) = (2x+2y+z-20, -3x-y-z+18, x+y+2z-16) is", Test, root, x0, eps);

		WriteLine("\nThe partial derivatives of Rosenbrock's valley funciton is:");
		WriteLine("		∂f/∂x = -2(1-x)-200x(y-x²)");
		WriteLine("		∂f/∂y = 200(y-x²)");

		x0 = new vector("1.5, 1.5");
		root = Newton(Rosenbrock, x0, eps);
		PrintResult("these are", Rosenbrock, root, x0, eps);
	}
	static void PrintResult(string function, Func<vector,vector> f, vector solution, vector guess, double eps)
	{
		string solutionFormatted = "";
		string guessFormatted = "";
		string resultFormatted = "";
		for(int i=0;i<solution.size;i++)
		{ 
			solutionFormatted += $"{solution[i]} ";
			guessFormatted += $"{guess[i]} ";
			resultFormatted += $"{f(solution)[i]} ";
		}
		WriteLine($"\nRoot of {function} ≈ {solutionFormatted},"); 
		WriteLine($" as f({solutionFormatted}) = {resultFormatted}");
		WriteLine($"	Initial guess was {guessFormatted}and the accuracy on f(x) was {eps}\n");   
		WriteLine(new string('-',100));
	}
}
