using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		/*
		int i=1; while (i+1 > i) i++;
		WriteLine($"My highest integer is {i}");
		WriteLine($"The correct value is  {int.MaxValue}");

		i = -1; while (i-1 < i) i--;
		WriteLine($"My lowest integer is {i}");
		WriteLine($"The correct value is {int.MinValue}");
		*/
		/*
		double x=1; while (x+1 != 1) x/=2;
		WriteLine(x);
		float y=1f; while ((float)(1f + y) != 1f) y/=2f;
		WriteLine(y);
		*/
		int n = (int)1e6;
		double epsilon = Pow(2,-52);
		double tiny = epsilon/2;
		double sumA = 0, sumB = 0;

		sumA++1; for(int i=0; i<n; i++) sumA+=tiny;
		for(int i=0; i<n; i++) sumB+=tiny; sumB++;
		WriteLine($"sum of A = {sumA}");
		WriteLine($"sum of B = {sumB}");
	}
}

