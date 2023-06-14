using static System.Console;

public static class main
{
	public static void Main()
	{
		vec u = new vec(1,2,3);
		u.print("u=");
		vec v = new vec(2,3,4);
		v.print("v=");
		vec k = v+u;
		k.print("v+u=");
		vec w = u*2;
		w.print("u*2=");
		(-u).print("-u=");
		WriteLine($"u*v = {u*v}");
		WriteLine($"u.dot(v) = {u.dot(v)}");
		vec crossed = u.cross(v);
		crossed.print("u cross v = ");
		WriteLine($"|u| = {u.norm()}");
		vec almostU = new vec(1.000000000001, 2.000000000001, 3.0000000000001);
		WriteLine($"u approx v ? {u.approx(v)}");
		almostU.print("almost-u: ");
		WriteLine($"u approx almost-u ? {u.approx(almostU)}");
		WriteLine($"Vector u as string: {u.ToString()}");
	}
}
