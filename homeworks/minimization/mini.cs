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
	public static (vector,int) amoeba(Func<vector,double> f, vector x, double acc=1e-3, double initialSize=0.1, int maxIterations=10000)
	{
		count = 0;
		int n = x.size;
		matrix points = new matrix(n, n+1); // columns contain the different points 
		points[0] = x;
		for(int i=0;i<n;i++)
		{
			vector point = x.copy();
			point[i] += initialSize;
			points[i+1] = point;
		}
		do
		{
			count++;
			if(count > maxIterations) throw new ArgumentException($"Maximum number of iterations reached, {maxIterations}");
			vector high = points[0];
			vector low = points[0];
			vector centroid = points[0];
			int highIndex = 0, lowIndex = 0;
			for(int i=1;i<n+1;i++) 
			{
				if(f(high) < f(points[i])) {high = points[i];highIndex=i;} 
				if(f(low) > f(points[0])) {low = points[i];lowIndex=i;}
				centroid+=points[i];
			}
			centroid = (centroid-high)/n;
			double A = 0;
			for(int i=0;i<n+1;i++) // Convergence if maxmimum distance between 2 points is less than acc
				for(int j=0;j<n+1;j++)
					if(j>i){vector diff=points[i]-points[j];A = Max(A,diff.norm());}
			if(A < acc) return (points[lowIndex], count);
			
			vector reflection = 2*centroid - high;
			if(f(reflection) < f(low))
			{
				vector expansion = 3*centroid - high;
				if(f(expansion) < f(reflection)) points[highIndex] = expansion;
				else points[highIndex] = reflection;
			}
			else if(f(reflection) < f(high)) points[highIndex] = reflection;
			else
			{
				vector contraction = 0.5*(centroid+high);
				if(f(contraction) < f(high)) points[highIndex] = contraction;
				else for(int i=0;i<n+1;i++) if(i != lowIndex) points[i] = 0.5*(points[i]+points[lowIndex]);
			}
			
			
		}while(true);
	
	}
}
