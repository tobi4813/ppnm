using static System.Math;

public static class static_sfuns1
{
	public static double gamma(double x)
	{
		///single precision gamma function (formula from Wikipedia)
		if(x<0)return PI/Sin(PI*x)/gamma(1-x); // Euler's reflection formula
		if(x<9)return gamma(x+1)/x; // Recurrence relation
		double lngamma=x*Log(x+1/(12*x-1/x/10))-x+Log(2*PI/x)/2;
		return Exp(lngamma);
	}
	public static double lngamma(double x)
	{
		if (x <= 0) return double.NaN;
		if (x < 9) return lngamma(x+1) - Log(x);
		return x * Log(x+1 / (12 * x-1/x/10)) - x+Log(2*PI/x)/2;

	}
}
