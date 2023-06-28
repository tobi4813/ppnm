using System;
using System.IO;

public partial class Spline
{
	public double[] x,y,b,c,d;
	public double A;
	
	public Spline(double[] x,double [] y)
	{
		this.x = x;
		this.y = y;
	}
	(genlist<double>,genlist<double>) Graph(Func<int,double,double> S, int resolution) // S(i,x) = ...
	{
		genlist<double> xs = new genlist<double>();
		genlist<double> ys = new genlist<double>();
		for(int i=0;i<x.Length-1;i++)
			for(int j=0;j<resolution;j++)
			{
				double x = this.x[i] + (this.x[i+1] - this.x[i])/(resolution-1)*j;
				xs.add(x);
				ys.add(S(i,x));
			}
		return (xs,ys);
	}
}
