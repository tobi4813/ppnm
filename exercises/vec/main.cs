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
		WriteLine($"u*v = {u.dot(v)}");
		vec crossed = u.cross(v);
		crossed.print("u x v = ");
		WriteLine($"|u| = {u.norm()}");
	}
}
