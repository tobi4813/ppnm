using System;
using static System.Math;

public static class solveODE
{	
	//public static SolveODE( if xlist!=null, method etc etc.
	static (vector, vector) rkstep12(Func<double,vector,vector> f, double x, vector y, double h)
	{
		vector k0 = f(x,y);
		vector k1 = f(x+h/2, y+k0*(h/2));
		vector yh = y+k1*h;
		vector er = (k1-k0)*h;
		return (yh, er);
	}
	static (vector, vector) rkstep45(Func<double,vector,vector> f, double x, vector y, double h)
	{
		
		vector c = new vector($"0 {1f/4} {3f/8} {12f/13} 1 {1f/2}"); 
		matrix a = new matrix($"0 0 0 0 0 0;
								{1f/4} 0 0 0 0 0;
								{3f/32} {9f/32} 0 0 0 0;
								{1932f/2197} {-7200f/2197} {7296f/2197} 0 0 0;
								{439f/216} -8 {3680f/513} {-845f/4104} 0 0;
								{-8f/27} 2 {-3544f/2565} {1859f/4104} {-11f/40} 0");
		vector b = new vector($"{16f/135} 0 {6656f/12825} {28561f/56430} {-9f/50} {2f/55}");
		vector bStar = new vector($"{25f/216} 0 {1408f/2565} {2197f/4104} {-1f/5} 0");
		matrix allK = new matrix(y.size, b.size);
		vector k = new vector(y.size);
		vector kStar = new vector(y.size);
		
		for(int i=0;i<b.size;i++)
		{
			double dx = c[i]*h;
			vector dy = new vector(y.size);
			for (int j=0;j<i;j++) dy+= a[j,i]*((vector)allK[j]);
			allK[i]=f(x+dx,y+dy);
			k+=b[i]*allK[i];
			kStar+=bStar[i]*allK[i];
		}
		vector yh = y+h*k;
		vector er = h*k - h*kStar;
		return (yh, er);
	}
	public static (vector, vector) step(Func<double,vector,vector> f, double x, vector y, double h, string stepper="rkf45")
	{
		switch(stepper)
		{
			case "rk12":
				return rkstep12(f,x,y,h);
			default:
				return rkstep45(f,x,y,h);
		}
	}
	
	public static (genlist<double>, genlist<vector>) drive(Func<double,vector,vector> f,double x0,vector y0,double xf,double h=0.01,
																double acc=1e-8,double eps=1e-8) //xf = xfinal, y0 = y values at x[0]
	{
		if(x0 > xf) throw new ArgumentException("driver: x0>xf");
		double x = x0;
		vector y = y0.copy();
		var xlist = new genlist<double>(); xlist.add(x);
		var ylist = new genlist<vector>(); ylist.add(y);
		do
		{
			if(x >= xf) return (xlist, ylist);
			if(x+h > xf) h = xf-x; // reduces h to not overshoot xf
			(vector yh,vector erv) = rkstep45(f,x,y,h);
			double tol = Max(acc, yh.norm()*eps) * Sqrt(h/(xf-x0));
			double err = erv.norm();
			if(err <= tol)
			{
				x+=h;
				y=yh;
				xlist.add(x);
				ylist.add(y);
			}
			h *= Min(Pow(tol/err, 0.25)*0.95, 2);
		}while(true);
	}

	public static vector driver(Func<double,vector,vector> f,double x0,vector y0,double xf,double h=1e-2,double acc=1e-8,
								double eps=1e-8,genlist<double> xlist=null,genlist<vector> ylist=null)
	{
		if(x0 > xf) throw new ArgumentException("driver: x0>xf");
		double x = x0;
		vector y = y0.copy();
		if(xlist!=null) xlist.add(x);
		if(ylist!=null) ylist.add(y);
		do
		{
			if(x >= xf) return y;
			if(x+h > xf) h = xf-x; // reduces h to not overshoot xf
			(vector yh,vector erv) = rkstep45(f,x,y,h);
		//	double[] tol = new double[y.size];
		//	for(int i=0;i<y.size;i++)
		//		tol[i] = Max(acc, Abs(y0
			double tol = Max(acc, yh.norm()*eps) * Sqrt(h/(xf-x0));
			double err = erv.norm();
			if(err <= tol)
			{
				x+=h;
				y=yh;
				if(xlist!=null) xlist.add(x);
				if(ylist!=null) ylist.add(y);
			}
			h *= Min(Pow(tol/err, 0.25)*0.95, 2);
		}while(true);
	
	}
}

