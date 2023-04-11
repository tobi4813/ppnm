using System;
using System.IO;
using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		vector time = new vector(new double[] {1,2,3,4,6,9,10,13,15});
		vector activity = new vector( new double[] {117,100,88,72,53,29.5,25.2,15.2,11.1});
		Func<double,double>[] fs = new Func<double,double>[] {z => 1.0 /*c=ln(a)*/, z => -z /*c=lambda*/};
		vector dy = new vector(new double[] {5,5,5,4,4,3,3,2,2});

		vector reducedActivity = new vector(activity.size);
		vector reducedDy = new vector(dy.size);
		for(int i=0;i<activity.size;i++){reducedActivity[i] = Log(activity[i]); reducedDy[i] = dy[i]/activity[i];}
	
		(vector parameters, matrix covariance) = Fit.lsfit(fs,time,reducedActivity,reducedDy);
		covariance.print("Covariance matrix:");
		double a = parameters[0], lambda = parameters[1];
		double aError = Sqrt(covariance[0,0]), lambdaError = Sqrt(covariance[1,1]);

		WriteLine("Coefficients:");
		WriteLine($"a = {Round(a,4)}±{Round(aError,4)}");
		WriteLine($"λ = {Round(lambda,4)}±{Round(lambdaError,4)}0 days⁻¹");
		WriteLine($"T½ = {Round(Log(2)/lambda,4)}00±{Round(Log(2)*lambdaError/Pow(lambda,2),4)} days"); //dT½ = ln(2) d(Lambda) /lambda^2

		
		using (StreamWriter output = new StreamWriter("decayData.data", append: false))
		{	
			output.WriteLine("\"Experimental data\"");
			for(int i=0;i<time.size;i++) output.WriteLine($"{time[i]} {activity[i]} {dy[i]}");
		}
		using (StreamWriter output = new StreamWriter("decayFit.data", append: false))
		{
			output.WriteLine($"\"Least squares fit, T_{{1/2}} = {Round(Log(2)/parameters[1],4)}d\"");
			int resolution = 100;
			for(int i=0;i<resolution+1;i++)
			{
				double t = time[time.size-1]/resolution * i;
				double lny = 0;
				for(int j=0;j<parameters.size;j++) lny += parameters[j]*fs[j](t);
				output.WriteLine($"{t} {Exp(lny)}");
			}
		}

	}
}
