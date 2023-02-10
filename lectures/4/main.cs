using System;
using static System.Console;
using static System.Math;
public static class main
{
	public static void Main()
	{
		int n = 9;
		double [] a = new double[9];
		for(int i=0;i<n;i++)
			Write($"a[{i}]={a[i]} ");
		WriteLine();
		for(int i = 0; i < n; i++)
			a[i] = i;
		foreach(double ai in a)
			Write($"{ai} ");
		WriteLine();
	}
}
