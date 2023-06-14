using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		///////// 1 ///////////////////////////////////////////////
		int i=1; while (i+1 > i) i++;
		WriteLine($"My highest integer is {i}");
		WriteLine($"The correct value is  {int.MaxValue}");

		i = -1; while (i-1 < i) i--;
		WriteLine($"My lowest integer is {i}");
		WriteLine($"The correct value is {int.MinValue}");

		///////// 2 ///////////////////////////////////////////////
		double x = 1; while (x+1 != 1) x/=2;
		x *= 2; 
		WriteLine($"\nMachine epsilon for doubles: {x}");
		WriteLine($"Should be close to {Pow(2,-23)}");
		float y = 1f; while ((float)(1f + y) != 1f) y/=2f;
		y *= 2f;
		WriteLine($"Machine epsilon for floats: {y}");
		WriteLine($"Should be close to {Pow(2,-52)}");

		///////// 3 ///////////////////////////////////////////////
		int n = (int)1e6;
		double epsilon = Pow(2,-52);
		double tiny = epsilon/2;
		double sumA = 1, sumB = 0;

		for(int j=0;j<n;j++) {sumA+=tiny; sumB+=tiny;}
		sumB++;
		WriteLine($"\nsumA - 1 = {sumA-1:e}");
		WriteLine($"Should be  {n*tiny:e}");
		WriteLine($"sumB - 1 = {sumB-1:e}");
		WriteLine($"Should be  {n*tiny:e}");
	
		///////// 4 ///////////////////////////////////////////////
		double d1 = 0.1+0.1+0.1+0.1+0.1+0.1+0.1+0.1;
		double d2 = 8*0.1;
		
		WriteLine($"\nd1={d1:e15}");
		WriteLine($"d2={d2:e15}");
		WriteLine($"d1==d2 ? => {d1==d2}");	
		WriteLine($"d1 approx d2 ? => {approx(d1,d2)}");


	}
	static bool approx(double a, double b, double acc=1e-9, double eps=1e-9)
	{
		if (Abs(a-b) < acc || Abs(b-a) < Max(Abs(a),Abs(b))*eps) return true;
		return false;
	}
}

