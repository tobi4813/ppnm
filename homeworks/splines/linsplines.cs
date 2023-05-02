using System;
using System.IO;

public partial class Spline
{
	public void LinterpIntegral(double z)
	{
		int i = 0;
		double integral = 0;
		while(x[i+1] < z) // integrates all intervals up to the k'th one from x[i] to x[i+1] and the k'th interval from x[k] to z
		{
			double dx = x[i+1]-x[i];
			integral += (y[i] + y[i+1])*dx/2; // same formula as the analytical integral y[i]*dx + dy/dx * dx²/2 = y[i]*dx + (y[i] - y[i])*dx/2 = (y[i] + y[i+1])*dx/2
			i++;
		}
		integral += (y[i] + Linterp(z))*(z-x[i])/2;
		A = integral;
	}
	double Linterp(double z)
	{
		int i = Binsearch(z);
		double dx = x[i+1]-x[i];
		double dy = y[i+1]-y[i];
		return y[i]+dy/dx*(z-x[i]);
	} 
	int Binsearch(double z)
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
