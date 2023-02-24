using static System.Console;
using static System.Math;

public static class main
{
	public static void Main()
	{
		WriteLine("hello");
		complex I = new complex(0,1);
		double a = -1;
		double b = 0;
		complex z = a+b*I;
		z.print("complex number z=");
		cmath.sqrt(z).print("sqrt(-1)=");
		if (z.approx(new complex(-1,0))) Write("sqrt(-1) very close to -i\n\n");
		else Write("Not close enough to -i\n\n");
		
		z = I;
		z.print("complex number z=");
		cmath.exp(z).print("exp(z)=");

		complex expIPi = cmath.exp(z*PI); 	
		expIPi.print("\nexp(i*PI)=");
		if (expIPi.approx(new complex(-1,0))) Write("exp(i*pi) Very close to -1+0i\n\n");
		else Write("Not close enough to -1+0i\n\n");

		complex iSquared = cmath.pow(z,z);
		iSquared.print("i^i=");
		if (iSquared.approx(Exp(-PI/2))) Write($"i^i Very close to exp(-pi/2)={Exp(-PI/2)}\n\n");
		else Write($"Not close enough to exp(-pi/2)={Exp(-PI/2)}\n\n");

		complex logI = cmath.log(z);
		logI.print("ln(i)=");
		if (logI.approx(new complex(0, PI/2))) Write($"ln(i) Very close to i*Pi/2={PI/2}i\n\n");
		else Write($"Not close enough i*Pi/2={PI/2}i\n\n");

		complex sinPiI = cmath.sin(PI*I);
		sinPiI.print("sin(i*Pi)=");
		if (sinPiI.approx(I*Sinh(PI))) Write($"sin(i*Pi) Very close to sinh(pi)*i={Sinh(PI)}i\n\n");
		else Write($"Not close enough to sinh(pi)*i={Sinh(PI)}i\n\n");
	}
}
