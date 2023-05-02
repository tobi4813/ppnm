using System;
using System.IO;
using static System.Math;

public partial class Spline
{
	public void Qspline()
	{
		b = new double[x.Length-1];
		c = new double[x.Length-1];
		Qspline_build(x,y,b,c);
	}
	void Qspline_build(double[] x, double[] y, double[] b, double [] c)
	{

		int n = x.Length;
		double[] dx = new double[n-1];
		double[] p = new double[n-1];
		for(int i=0;i<n-1;i++)
		{
			dx[i] = x[i+1] - x[i];
			p[i] = (y[i+1] - y[i])/dx[i];
		}
		c[0] = 0;
		for(int i=0;i<n-2;i++) c[i+1] = (p[i+1] - p[i] - c[i]*dx[i])/dx[i+1];
		c[n-2] /= 2;
		b[n-2] = p[n-2] - c[n-2]*dx[n-2];
		for(int i=n-3;i>=0;i--) {c[i] = (p[i+1]-p[i]-c[i+1]*dx[i+1])/dx[i]; b[i] = p[i] - c[i]*dx[i];}
	}
	public void Qintegral(double z)
	{
		int i = 0;
		double integral = 0;
		while(x[i+1] < z)
		{
			double dx = x[i+1] - x[i];
			integral += y[i]*dx + b[i]*dx*dx/2 + c[i]*dx*dx*dx/3;
			i++;
		}
		integral += y[i]*(z-x[i]) + b[i]*(z-x[i])*(z-x[i])/2 + c[i]*Pow(z-x[i],3)/3;
		A = integral;
	}
	public (genlist<double>,genlist<double>) Qgraph(int resolution)
	{
		Func<int,double,double> S = (i,x) => y[i] + b[i]*(x-this.x[i]) + c[i]*(x-this.x[i])*(x-this.x[i]);
		return Graph(S,resolution);
	}
	public (genlist<double>,genlist<double>) Qderivative(int resolution)
	{
		Func<int,double,double> S = (i,x) => b[i] + 2*c[i]*(x-this.x[i]);
		return Graph(S,resolution);
	}
	
}
