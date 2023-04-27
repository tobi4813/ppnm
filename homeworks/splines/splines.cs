using System;
using System.IO;

public static class Spline
{
	public static (double[], double[]) Qspline(double[] x, double[] y)
	{
		double[] b = new double[x.Length-1];
		double[] c = new double[x.Length-1];
		Qspline_build(x,y,b,c);
		return (b,c);
	}
	static void Qspline_build(double[] x, double[] y, double[] b, double [] c)
	{
	using (StreamWriter output = new StreamWriter("test.txt"))
	{
		int n = x.Length;
		double[] dx = new double[n-1];
		double[] dy = new double[n-1];
		double[] p = new double[n-1];
		output.WriteLine("Before first loop");
		for(int i=0;i<n-1;i++)
		{
			dx[i] = x[i+1] - x[i];
			p[i] = (y[i+1] - y[i])/dx[i];
		}
		output.WriteLine("Just after first loop");
		c[0] = 0;
		output.WriteLine("Before 2nd loop");
		for(int i=0;i<n-2;i++) c[i+1] = (p[i+1] - p[i] - c[i]*dx[i])/dx[i+1];
		output.WriteLine("Just after 2nd loop");
		c[n-2] /= 2;
		b[n-2] = p[n-2] - c[n-2]*dx[n-2];
		output.WriteLine("Before 3rd loop");
		for(int i=n-3;i>=0;i--) {c[i] = (p[i+1]-p[i]-c[i+1]*dx[i+1])/dx[i]; b[i] = p[i] - c[i]*dx[i];}
		output.WriteLine("After 3rd loop");		
	}}
	
	/*public static void Qspline_build(double[] x, double[] y, double[] b, double [] c)
	{
		int n = x.Length;
		c[0] = 0;
		
		double dx0 = x[1] - x[0];
		double dy0 = y[1] - y[0];
		double p0 = dy0/dx0

		for(int i=0;i<n-2;i++) // Forward recursion
			double dx1 = x[i+2] - x[i+1];
			double dy1 = x[i+2] - x[i+1];
			double p1 = dy1/dx1;

			c[i+1] = 1/dx1 * (p1 - p0 - c[i]*dx0) // dx0 = dx_i, dx1 = dx_i+1

			dx0 = dx1;
			p0 = p1;
		}
		c[n-2] /= 2; 

		dx0 = x[n-1] - x[n-2];
		dy0 = y[n-1] - y[n-2];
			
		for(int i=n-3;i>=0;i--)
		{
			double dx1 = x[i+1] - x[i];
			double dy1 = x[i+1] - x[i];
			double p1 = dy1/dx1;

			c[i] = 1/dx1 * (p0 - p1 - c[i+1]*dx0) // dx1 = dx_i, dx0 = dx_i+1

			dx0 = dx1;
			p0 = p1;
		}
	}*/
	
	
	public static double LinterpIntegral(double[] x, double[] y, double z)
	{
		int i = 0;
		double integral = 0;
		
		while(x[i+1] < z) // integrates all intervals up to the k'th one from x[i] to x[i+1] and the k'th interval from x[k] to z
		{
			double dx = x[i+1]-x[i];
			integral += (y[i] + y[i+1])*dx/2; // same formula as the analytical integral y[i]*dx + dy/dx * dx²/2 = y[i]*dx + (y[i] - y[i])*dx/2 = (y[i] + y[i+1])*dx/2
			i++;
		}
		integral += (y[i] + Linterp(x,y,z))*(z-x[i])/2;
		return integral;
	}
	static double Linterp(double[] x, double[] y, double z)
	{
		int i = Binsearch(x,z);
		double dx = x[i+1]-x[i];
		double dy = y[i+1]-y[i];
		return y[i]+dy/dx*(z-x[i]);
	} 
	static int Binsearch(double[] x, double z)
	{
		if(!(x[0] <= z && z <=x[x.Length-1])) throw new ArgumentException("Binsearch: z no good");
		int i = 0, j = x.Length-1;
		while(j-i>1)
		{
			int mid = (i+j)/2;
			if(z>x[mid]) i = mid; else j = mid;
		}
		return i;
	}
}
