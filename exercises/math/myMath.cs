using static System.Console;
using static System.Math;

class myMath
{
	static void Main()
	{
		Write($"Squareroot 2 is {Sqrt(2)}.");
		Write(" Proof: " + Sqrt(2) + " * " + Sqrt(2) + " = " + Sqrt(2)*Sqrt(2));

		Write(" Proof: "+ Pow(2, 1f/5f) + "*" + Pow(2, 1f/5f) + "*" + Pow(2, 1f/5f) + "*" + Pow(2, 1f/5f) + "*" + Pow(2, 1f/5f) + "="
						+ Pow(2, 1f/5f)*Pow(2, 1f/5f) * Pow(2, 1f/5f)*Pow(2, 1f/5f)*Pow(2, 1f/5f));
		Write("\n" +static_sfuns1.gamma(1));
		Write("\n" +static_sfuns1.gamma(2));
		Write("\n" +static_sfuns1.gamma(3));
		Write("\n" +static_sfuns1.gamma(10));
	}
}
