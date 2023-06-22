using System;
using System.IO;
using static System.Math;

public static class MC
{
	public static (double,double) PlainMC(Func<vector,double> f, vector a, vector b, int N, genlist<double> xs=null, genlist<double> ys=null)
	{
		int dim = a.size;
		double V = 1;
		for(int i=0;i<dim;i++) V *= b[i] - a[i];
		double sum = 0, sum2 = 0;
		vector x = new vector(dim);
		Random rnd = new Random();
		for(int i=0;i<N;i++)
		{
			for(int k=0;k<dim;k++) x[k] = a[k] + rnd.NextDouble()*(b[k] - a[k]);
			double fx = f(x);
			sum += fx;
			sum2 += fx*fx;
			if(xs != null) {xs.add(x[0]); ys.add(x[1]);}
		}
		double mean = sum/N;
		double sigma = Sqrt(sum2/N - mean*mean);
		return (mean*V, sigma*V/Sqrt(N));
	}
	public static (double,double) QuasiMC(Func<vector,double> f, vector a, vector b, int N, genlist<double> xs=null, genlist<double> ys=null, int offset=0)
	{
		int dim = a.size;
		double V = 1;
		for(int i=0;i<dim;i++) V *= b[i] - a[i];
		double sum = 0, sum2 = 0;
		vector x = new vector(dim);
		vector x2 = new vector(dim);
		for(int n=0;n<N;n++)
		{
			for(int k=0;k<dim;k++) 
			{
				x[k] = a[k] + Corput(n,Prime(k+offset))*(b[k] - a[k]);
				x2[k] = a[k] + Corput(n,Prime(k+7+offset))*(b[k] - a[k]);
			}
			if(xs != null) {xs.add(x[0]); ys.add(x[1]);} 
			sum += f(x);
			sum2 += f(x2);
		}
		double mean = sum/N;
		double mean2 = sum2/N;
		double err = V*Abs(mean-mean2);
		return (mean*V, err);
	}
	static double Corput(int n, int b)
	{
		double q = 0, bk = (double)1/b;
		while(n > 0) {q += (n % b)*bk; n /= b; bk /= b;}
		return q;
	}
	static void Halton(int n, int d, vector x)
	{
		int[] base_ = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		int maxd = base_.Length/sizeof(int);
		if(d > maxd) for(int i=0;i<d;i++) x[i] = Corput(n,base_[i]);
	}	
	static int Prime(int i)
	{
		int[] primes = {2,3,5,7,11,13,17,19,23,29,31,37,41,43,47,53,59,61};
		if(i >= primes.Length) return Prime(i-primes.Length);
		else return primes[i];
	}
	public static (double,double) StratifiedMC(Func<vector,double> f, vector a, vector b, int N, int nmin=100, genlist<double> xs=null, genlist<double> ys=null)
	{
		if (N < nmin) {return PlainMC(f,a,b,Max(N,1),xs,ys);} // If N = 0 it means the value is likely the same in the entire volume, so a single point will return that value
		int dim = a.size;
		double[] errs = new double[dim];
		double[] ratio = new double[dim];
		//double[] le = new double[dim];
		//double[] re = new double[dim];
		for(int i=0;i<dim;i++)
		{
			double Mid = (a[i] + b[i])/2;
			vector MidLeft=b.copy(), MidRight=a.copy();
			MidLeft[i] = Mid;
			MidRight[i] = Mid;
			int n = Max((int) (N/10), 200);
			double LeftErr = PlainMC(f,a,MidLeft,n).Item2;
			double RightErr = PlainMC(f,MidRight,b,n).Item2;
		//	le[i] = LeftErr;
		//	re[i] = RightErr;
			errs[i] =Abs( LeftErr*LeftErr - RightErr*RightErr);
			ratio[i] = LeftErr/(LeftErr+RightErr);
		}

		int largestErrDim = 0;
		for(int i=1;i<dim;i++) if(errs[i] > errs[largestErrDim]) largestErrDim = i;
		double mid = (a[largestErrDim] + b[largestErrDim])/2;
		vector midLeft=b.copy(), midRight=a.copy();
		midLeft[largestErrDim] = mid;
		midRight[largestErrDim] = mid;
		int leftPoints = (int)(N * ratio[largestErrDim]);
		int rightPoints = N - leftPoints; 
		
		(double leftIntegral, double leftErr) = StratifiedMC(f,a,midLeft,leftPoints,nmin,xs,ys);
		(double rightIntegral, double rightErr) = StratifiedMC(f,midRight,b,rightPoints,nmin,xs,ys);
		/*using (var output = new StreamWriter("debug.txt", append: true))
		{
			output.WriteLine();
			output.WriteLine($"left block:");
			output.WriteLine($"(x1, x2) {a[0]} {midLeft[0]}");
			output.WriteLine($"(y1, y2) {a[1]} {midLeft[1]}");
			
			output.WriteLine($"right block:");
			output.WriteLine($"(x1, x2) {midRight[0]} {b[0]}");
			output.WriteLine($"(y1, y2) {midRight[1]} {b[1]}");
			
			output.WriteLine($"errs:\n {le[largestErrDim]}\n {re[largestErrDim]}");
			output.WriteLine($"points: {leftPoints} {rightPoints}");
			output.WriteLine($"Splitting on x,y errs:\n {errs[0]} \n {errs[1]}");
		}*/
		double grandErr = Sqrt(leftErr*leftErr + rightErr*rightErr);
		return (leftIntegral + rightIntegral, grandErr); // i any of the points are 0, that means the error is 0, but 0/0 is NaN, so with Max, it returns 0 instead of NaN
	}
}

