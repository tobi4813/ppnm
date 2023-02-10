using static System.Console;

public class vec
{
	public double x,y,z;
	public vec (double a, double b, double c)
	{
		x=a;y=b;z=c;
	}	
	public vec ()
	{
		x = 0; y = 0; z= 0;
	}
	public void print(string s)
	{
		Write(s);
		WriteLine($"{x} {y} {z}");
	}
	public static vec operator+(vec u, vec v)
	{
		return new vec(u.x + v.x, u.y + v.y, u.z + v.z);
	}
	public static vec operator-(vec u, vec v)
	{
		return new vec(u.x - v.x, u.y - v.y, u.z - v.z);
	}
	public static vec operator*(vec u, double c)
	{
		return new vec(u.x * c, u.y * c, u.z * c);
	}
	public static vec operator*(double c, vec u)
	{
		return u*c;
	}
	public static vec operator-(vec u)
	{
		return u*(-1);
	}
	public static double operator% (vec u, vec v)
	{
		return u.x * v.x + u.y * v.y + u.z * v.z;
	}
}
