using static System.Console;
class main
{
	static void Main(string[] args)
	{
		foreach(var arg in args)
		{
			if(arg == "gamma")	
				for(double x=-5+1.0/128; x<=5; x+=1.0/64)
				{
					WriteLine($"{x} {sfuns.gamma(x)}");
				}
			else if(arg == "erf")
				for(double x=-2+1.0/128; x<=3; x+=1.0/64)
				{
					WriteLine($"{x} {sfuns.erf(x)}");
				}
		}
	}
}
