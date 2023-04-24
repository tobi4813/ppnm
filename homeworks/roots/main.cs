using System;
using System.IO;
using System.Threading;
using static System.Console;
using static Roots;
using static System.Math;
using static solveODE;
using static System.Math;

static class main
{
	static void Main()
	{
		// A)
		Tests(); 
		
		// B)
		double rmin = 0.01, rmax=8;
		genlist<double> rs = new genlist<double>();
		genlist<vector> fs = new genlist<vector>();
		vector E0guess = new vector(-0.7);
		vector E0 = Newton(M(rmin, rmax), E0guess);
		M(rmin, rmax, rs, fs)(E0); // saves data for the correct energy
		Directory.CreateDirectory("data");
		using (StreamWriter output = new StreamWriter("data/hydrogenNumerical.data"))
		{
			output.WriteLine($"r \"E_0  found numerically to {Round(E0[0],3)}\"");
			for(int i=0;i<rs.size;i++) output.WriteLine($"{rs[i]} {fs[i][0]} {fs[i][1]}");
		}
		Write("---< B) >");WriteLine(new string('-', 91));
		WriteLine("\nThe derivative of r exp(-r) is (1-r)exp(-r), so the 2nd derivative is (r-2)exp(-r), so:");
		WriteLine("   -0.5(r-2)exp(-r) - (1/r)r exp(-r) = -0.5(r-2+2)exp(-r)");
		WriteLine("                                     = -0.5r exp(-r)");
		WriteLine("                                ⇒ E₀ = -0.5   ");

		// B) convergence
		WriteLine("execution time for A) and B):");
		// Preparing multithreading
		int nthreads = 4;
		Thread[] threads = new Thread[nthreads]; 

		int resolution = 100; // if resolution is not divisibleby nthreads, the remaing < nthreads-1 points will not be evaluated
		// default values when not the variable
		rmin = 0.05; 
		rmax = 8; 
		double acc = 1e-4, eps = 1e-4;
		
		// saving data in an array: [ [rmins, E0s], [rmaxs, E0s], [accs, E0s], [epss, E0s] ]
		double[][,] data = new double[4][,];
		string[] dataNames = {"rmin", "rmax", "acc", "eps"};
		double[][] limits = { /* last point not included*/
					new double[] {0.001,0.4}, 
					new double[] {1,8}, 
					new double[] {Log10(1e-1), Log10(1e-6)}, 
					new double[] {Log10(1e-1), Log10(1e-9)}}; // limits for the parameters when testing convergence
		double[] guesses = {-0.7, -0.5, -0.7, -0.7};
		for(int j=0;j<4;j++) // 4 dataset 
		{	
			data[j] = new double[2,resolution];
			for(int i=0;i<nthreads;i++) // multithreading
			{
				Params parameters = new Params(nthreads,i,resolution,limits[j],guesses[j],rmin,rmax,acc,eps,data[j],dataset: dataNames[j]);
				threads[i] = new Thread(Convergence);
				threads[i].Start(parameters);
			}
			for(int i=0;i<nthreads;i++) threads[i].Join();
			using (StreamWriter output = new StreamWriter($"data/{dataNames[j]}Convergence.data"))
			{
				output.WriteLine($"dataNames[j] E_0");
				for(int i=0;i<resolution;i++) output.WriteLine($"{data[j][0,i]} {data[j][1,i]}");
			}
		}
	}
	static void Convergence(object parameter)
	{
			// initializing all 7 million parameters
			Params parameters = (Params) parameter;
			int nthreads = parameters.nthreads, t = parameters.t, resolution = parameters.resolution;
			double[] limits = parameters.limits;
			double rmin = parameters.rmin, rmax = parameters.rmax, acc = parameters.acc, eps = parameters.eps;
			double[,] data = parameters.data;
			string dataset = parameters.dataset;
			vector E0guess = new vector(parameters.guess); //-0.7
			vector E0;
			for(int j=0;j<resolution/nthreads;j++) //every n'th -> (n*i +0,1,2,...)
			{	
				int i = nthreads*j+t;
				double variable = limits[0] + (limits[1]-limits[0])/resolution*i;
				data[0,i] = variable;

				switch(dataset)
				{
					case "rmin":
						E0 = Newton(M(variable, rmax, acc: acc, eps: eps), E0guess, eps: 1e-4);
						break;
					case "rmax":
						variable = limits[0] + (limits[1]-limits[0])/resolution*i;
						E0 = Newton(M(rmin, variable, acc: acc, eps: eps), E0guess, eps: 1e-4);
						break;
					case "acc":
						variable = Pow(10,variable);
						E0 = Newton(M(rmin, rmax, acc: variable, eps: eps), E0guess, eps: 1e-4);
						break;
					case "eps":
						variable = Pow(10,variable);
						E0 = Newton(M(rmin, rmax, acc: acc, eps: variable), E0guess, eps: 1e-4);
						break;
					default:
						variable = limits[0] + (limits[1]-limits[0])/resolution*i;
						E0 = new vector(resolution);
						break;
				}
				data[1,i] = E0[0];
			}
	}
	static Func<double, vector, vector> Hydrogen(double E)
	{	
		Func<double, vector, vector> f = (r,y) => new vector(y[1], -2*(E+1/r)*y[0]);
		return f;
	}	
	static Func<vector,vector> M(double rmin, double rmax, genlist<double> rs=null, genlist<vector>ys=null, double acc=1e-4,double eps=1e-4)
	{
		vector f0 = new vector(rmin-rmin*rmin,1-2*rmin);
		Func<vector,vector> aux = E => new vector(driver(Hydrogen(E[0]), rmin, f0, rmax, acc: acc, eps: eps, xlist: rs, ylist: ys, method: "rkf45")[0]); // driver returns only the vector at the last point, so driver(...)[0] = FE(rmax)
		return aux;
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
	static void Tests()
	{
		
		double eps = 1e-3;
		vector x0 = new vector(1.0);
		vector root = Newton(Parabola(1,0,0), x0, eps);
		PrintResult("f(x) = x² is", Parabola(1,0,0), root, x0, eps);

		/*root = Newton(Parabola(1,0,1), x0, eps);
		PrintResult("f(x) = x²+1 is", Parabola(1,0,1), root, x0, eps);	eps = 1e-3;
		maxIterations error, as expected*/
		
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
public class Params
{
	public int nthreads, t, resolution;
	public double[] limits;
	public double guess, rmin, rmax, acc, eps;
	public double[,] data;
	public string dataset;
	public Params(int nthreads, int t, int resolution, double[] limits, double guess,
					double rmin, double rmax, double acc, double eps, double[,] data, string dataset)
	{
		this.nthreads = nthreads;
		this.t = t;
		this.resolution = resolution;
		this.limits = limits;
		this.guess = guess;
		this.rmin = rmin;
		this.rmax = rmax;
		this.acc = acc;
		this.eps = eps;
		this.data = data;
		this.dataset = dataset;
	}
}
