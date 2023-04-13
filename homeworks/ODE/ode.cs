using System;
using static System.Math;
using System.IO;

public static class solveODE
{	
	static (matrix, vector, vector, vector) midpointEulerTable()
	{
		matrix a = new matrix($"0 0; {0.5} 0");
		vector b = new vector("0 1");
		vector bStar =  new vector("1 0");
		vector c = new vector($"0 {0.5}");
		return (a,b,bStar,c);
	}
	static (matrix, vector, vector, vector) rkf45Table()
	{
		matrix a = new matrix($"0 0 0 0 0 0;
								{1f/4} 0 0 0 0 0;
								{3f/32} {9f/32} 0 0 0 0;
								{1932f/2197} {-7200f/2197} {7296f/2197} 0 0 0;
								{439f/216} -8 {3680f/513} {-845f/4104} 0 0;
								{-8f/27} 2 {-3544f/2565} {1859f/4104} {-11f/40} 0");
		vector b = new vector($"{16f/135} 0 {6656f/12825} {28561f/56430} {-9f/50} {2f/55}");
		vector bStar = new vector($"{25f/216} 0 {1408f/2565} {2197f/4104} {-1f/5} 0");
		vector c = new vector($"0 {1f/4} {3f/8} {12f/13} 1 {1f/2}"); 
		return (a,b,bStar,c);
	}
	static (vector, vector) rkstep45(matrix a, vector b, vector bStar, vector c, Func<double,vector,vector> f, double x, vector y, double h)
	{
		
		matrix allK = new matrix(y.size, b.size);
		vector k = new vector(y.size);
		vector kStar = new vector(y.size);
		
		for(int i=0;i<b.size;i++)
		{
			double dx = c[i]*h;
			vector dy = new vector(y.size);
			for (int j=0;j<i;j++) dy+= h*a[i,j]*((vector)allK[j]);
			
			allK[i]=f(x+dx,y+dy);
			k+=b[i]*allK[i];
			kStar+=bStar[i]*allK[i];
		}
		vector yh = y+h*k;
		vector er = h*k - h*kStar;
		return (yh, er);
	}
	public static (genlist<double>, genlist<vector>) drive(Func<double,vector,vector> f,double x0,vector y0,double xf,double h=0.01,
																double acc=1e-3,double eps=1e-3,string method="rkf45") //xf = xfinal, y0 = y values at x[0]
	{
		if(x0 > xf) throw new ArgumentException("driver: x0>xf");
		double x = x0;
		vector y = y0.copy();
		matrix a;
		vector b, bStar, c;
		switch(method)
		{
			case "rk12":
				(a,b,bStar,c) = midpointEulerTable();
				break;
			default:
				(a,b,bStar,c) = rkf45Table();
				break;
		}
		var xlist = new genlist<double>(); xlist.add(x);
		var ylist = new genlist<vector>(); ylist.add(y);
		do
		{
			if(x >= xf) return (xlist, ylist);
			if(x+h > xf) h = xf-x; // reduces h to not overshoot xf
			(vector yh,vector erv) = rkstep45(a,b,bStar,c,f,x,y,h);
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

	public static vector driver(Func<double,vector,vector> f,double x0,vector y0,double xf,double h=1e-2,double acc=1e-3,
								double eps=1e-3,genlist<double> xlist=null,genlist<vector> ylist=null,string method = "rkf45")
	{
		if(x0 > xf) throw new ArgumentException("driver: x0>xf");
		double x = x0;
		vector y = y0.copy();
		matrix a;
		vector b, bStar, c;
		switch(method)
		{
			case "rk12":
				(a,b,bStar,c) = midpointEulerTable();
				break;
			default:
				(a,b,bStar,c) = rkf45Table();
				break;
		}
		if(xlist!=null) xlist.add(x);
		if(ylist!=null) ylist.add(y);
		do
		{
			if(x >= xf) return y;
			if(x+h > xf) h = xf-x; // reduces h to not overshoot xf
			(vector yh,vector erv) = rkstep45(a,b,bStar,c,f,x,y,h);
			bool ok = true;
			double[] tol = new double[y.size];
			for(int i=0;i<y.size;i++)
			{
				tol[i] = Max(acc, eps*Abs(y0[i]))*Sqrt(h/(xf-x0));
				if(!(erv[i]<tol[i])) ok = false;
			}
			if(ok)
			{
				x+=h;
				y=yh;
				if(xlist!=null) xlist.add(x);
				if(ylist!=null) ylist.add(y);
			}
			double factor = tol[0]/Abs(erv[0]);
			for(int i=0;i<y.size;i++) factor = Min(factor, tol[i]/Abs(erv[i]));
			h *= Min(Pow(factor, 0.25)*0.95, 2);
		}while(true);
	}
}
