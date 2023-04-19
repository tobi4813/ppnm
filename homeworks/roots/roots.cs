using System;
using System.IO;
using static System.Math;

public static class Roots
{
	public static vector Newton(Func<vector,vector> f, vector x, double eps=1e-2)
	{
		int n = x.size;
		int j = 0;
		do
		{
			using (StreamWriter output = new StreamWriter("test.txt", append: true))
			{
			
			j++;
			if (j>10000) return 4*x;
			if (f(x).norm() < eps) return x;
			matrix J = new matrix(n,n);
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
			output.WriteLine(Dx.norm());
			output.WriteLine(dxs.norm());
			if (Dx.norm() < dxs.norm()) throw new ArgumentException("Newton: Δx<δx, solution not found");
			double lambda = 1; // define before first do?
			do
			{
				lambda /= 2;
				
			}while( f(x+lambda*Dx).norm() > (1-lambda/2)*f(x).norm() && lambda > 1/128 );	
			x += lambda*Dx;	
			output.WriteLine();
			}
		}while(true);
	}
}
