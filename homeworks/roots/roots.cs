using System;
using System.IO;
using static System.Math;

public static class Roots
{
	public static vector Newton(Func<vector,vector> f, vector x, double eps=1e-2, int maxIterations=1000)
	{
		int n = x.size;
		int j = 0;
		//using (StreamWriter output = new StreamWriter("test.txt", append: true))
		//{
		//output.WriteLine("yo");
		do
		{
			//output.WriteLine("yo2");
			j++;
			if(j>=maxIterations) throw new ArgumentException($"Maxmimum number of iterations reached {j}");
			if (f(x).norm() < eps) return x;
			matrix J = new matrix(n,n); //outside?
			vector dxs = new vector(n);
			for(int k=0;k<n;k++)
			{
				double dx = Abs(x[k])*Pow(2,-26);
				dxs[k] = dx;
				vector xStep = x.copy();
				xStep[k] += dx;
				vector df = f(xStep) - f(x);
				for(int i=0;i<n;i++) J[i,k] = df[i]/dx;
			}
			QRGS JDx = new QRGS(J);
			vector Dx = JDx.solve(-f(x));
			//output.WriteLine(dxs.norm());
			//output.WriteLine($" x: {x[0]}");
			//output.WriteLine($"DX: {Dx[0]}");
			if (Dx.norm() < dxs.norm()) throw new ArgumentException("Newton: Δx<δx, solution not found");
			double lambda = 1; // define before first do?
			do
			{
				lambda /= 2;
			}while( f(x+lambda*Dx).norm() > (1-lambda/2)*f(x).norm() && lambda > 1f/128 );	
			x += lambda*Dx;	
			//output.WriteLine($" l: {lambda}");
			//output.WriteLine($"x1: {x[0]}");
			//output.WriteLine($" f: {f(x)[0]}");
			//output.WriteLine();
			
		}while(true);
	//	}
	}
}
