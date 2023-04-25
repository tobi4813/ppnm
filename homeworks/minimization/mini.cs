using System;
using static System.Math;
using static matrix;

public static class Minimization
{
	static int count = 0; 
	public static (vector,int) qnewtonCount(Func<vector,double> f, vector x, double acc=1e-3, int maxIterations=1000)
	{
		vector solution = qnewton(f,x,acc,maxIterations);
		return (solution, count);
	}
	public static vector qnewton(Func<vector,double> f, vector x, double acc=1e-3, int maxIterations=1000)
	{
		int n = x.size;
		matrix B = id(n);
		vector dx = new vector(n);
		vector gradient = new vector(n);
		vector gradientStep = new vector(n);
		count = 0;
		do
		{
			count++;
			if(count > maxIterations) throw new ArgumentException($"Maximum number of iterations reached, {maxIterations}");
			for(int i=0;i<n;i++)
			{
				dx[i] = Abs(x[i])*Pow(2,-26);
				vector xStep = x.copy();
				xStep[i] += dx[i];
				double df = f(xStep) - f(x);
				gradient[i] = df/dx[i];
			}
			if(gradient.norm() < acc) return x;
			vector nstep = -B*gradient;
			double lambda = 1;
	
			do
			{
				vector s = lambda*nstep;
				if(f(x+s) < f(x))
				{
					for(int i=0;i<n;i++)
					{
						vector xstep = x.copy();
						xstep[i] += dx[i];
						double df = f(xstep+s) - f(x+s);
						gradientStep[i] = df/dx[i];
					}
					vector y = gradientStep - gradient;
					double sTy = s%y;
					if(Abs(sTy) > 1e-6)
					{
						vector u = s - B*y;
						double gamma = (u%y)/(2*sTy);
						vector a = (u - gamma*s)/sTy;
						B += outer(a,s) + outer(s,a); // Symmetric Broyden's update
					}
					else B = id(n);
					x += s;
					break;
				}
				lambda /= 2;
				if(lambda < 1f/1024)
				{
					x += s;
					B = id(n);
					break;
				}
			}while(true);
		}while(true);
	}
}
