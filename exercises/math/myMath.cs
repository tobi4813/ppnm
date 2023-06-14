using static System.Console;
using static System.Math;

class myMath
{
	static void Main()
	{
		WriteLine($"Squareroot 2 is {Sqrt(2)}");
		WriteLine($" Proof: {Sqrt(2)}*{Sqrt(2)} = {Sqrt(2)*Sqrt(2)}");
		
		WriteLine($"\n2^(1/5) is {Pow(2,1f/5)}");
		WriteLine($" Proof: {Pow(2, 1f/5f)} * {Pow(2, 1f/5f)} * {Pow(2, 1f/5f)} * {Pow(2, 1f/5f)} * {Pow(2, 1f/5f)} = {Pow(2, 1f/5f)*Pow(2, 1f/5f)*Pow(2, 1f/5f)*Pow(2, 1f/5f)*Pow(2, 1f/5f)}");

		WriteLine($"\nexp(PI) is {Exp(PI)}");
		WriteLine($" since Math.Pow is working, proof: Pow(E,PI) = {Pow(E,PI)}");

		WriteLine($"\nPI^e is {Pow(PI,E)}\n");

		WriteLine($"Γ(1) = {static_sfuns1.gamma(1)}");
		WriteLine($" Exact: 1\n");
		WriteLine($"Γ(2) = {static_sfuns1.gamma(2)}");
		WriteLine($" Exact 1\n");
		WriteLine($"Γ(3) = {static_sfuns1.gamma(3)}");
		WriteLine($" Exact 2\n");
		WriteLine($"Γ(10) = {static_sfuns1.gamma(10)}");
		WriteLine($"  Exact {9*8*7*6*5*4*3*2}");
	}
}
