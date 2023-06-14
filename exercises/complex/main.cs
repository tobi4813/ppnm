using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		complex I = new complex(0,1);
		complex minusOne = new complex(-1,0);

		complex sqrtMinusOne = cmath.sqrt(minusOne);
		sqrtMinusOne.print("          sqrt(-1) = ");
		complex shouldBe = I;
		if (sqrtMinusOne.approx(shouldBe)) shouldBe.print("sqrt(-1) is actually ");
		else shouldBe.print("Not close enough to ");
		
		complex sqrtI = cmath.sqrt(I);
		sqrtI.print("\n          sqrt(i) = ");
		shouldBe = new complex(1/Sqrt(2), 1/Sqrt(2));
		if (sqrtI.approx(shouldBe)) shouldBe.print("sqrt(i) is actually ");
		else shouldBe.print("Not close enough to ");

		complex expI = cmath.exp(I);
		expI.print("\n          exp(i) = ");
		shouldBe = new complex(Cos(1), Sin(1));
		if (expI.approx(shouldBe)) shouldBe.print("exp(i) is actually ");
		else shouldBe.print("Not close enough to ");


		complex expIPi = cmath.exp(I*PI); 	
		expIPi.print("\n          exp(i*PI) = ");
		shouldBe = new complex(Cos(PI), Sin(PI));
		if (expIPi.approx(shouldBe)) shouldBe.print("exp(i*PI) is actually ");
		else shouldBe.print("Not close enough to ");

		complex iSquared = cmath.pow(I,I);
		iSquared.print("\n          i^i = ");
		shouldBe = new complex(Exp(-PI/2), 0);
		if (iSquared.approx(shouldBe)) shouldBe.print("i^i is actually ");
		else shouldBe.print("Not close enough to ");
		
		complex logI = cmath.log(I);
		logI.print("\n          ln(i) = ");
		shouldBe = new complex(0, PI/2);
		if (logI.approx(shouldBe)) shouldBe.print("ln(i) is actually ");
		else shouldBe.print("Not close enough to ");

		complex sinPiI = cmath.sin(PI*I);
		sinPiI.print("\n          sin(i*Pi) = ");
		shouldBe = new complex(0, Sinh(PI));
		if (sinPiI.approx(shouldBe)) shouldBe.print("sin(i*PI) is actually ");
		else shouldBe.print("Not close enough to ");
	}
}
