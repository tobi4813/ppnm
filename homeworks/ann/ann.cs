using System;
using static System.Math;
using static Minimization;

public class ANN
{
	int n;
	Func<double,double> f = x => x*Exp(-x*x);
	Func<double,double> fprime = x => Exp(-x*x) - 2*x*x*Exp(-x*x);
	Func<double,double> fprimeprime = x => Exp(-x*x)*(4*x*x*x - 6*x);
	Func<double,double> F = x => -Exp(-x*x)/2; // int f(x)dx = int x*exp(-x²) dx -> use x*dx = d(x²)/2 to get int f(x) = -exp(-x²)/2
	public vector p; // P = [a...,b...,w...]
	public double acc = 1e-3;
	double a(int i) {return p[i];}
	double b(int i) {return p[n+i];}
	double w(int i) {return p[2*n+i];}
	
	public ANN(int n)
	{
		this.n = n;	
		p = new vector(n);
	}
	public double response(double x)
	{
		double r = 0;
		for(int i=0;i<n;i++) r += w(i)*f((x-a(i))/b(i)); //p[i] € a_0...a_n, p[n+i] € b_0...b_n, p[2n+i] € w_0...w_n]
		return r;
	}
	public double Derivative(double x)
	{
		double y = 0;
		for(int i=0;i<n;i++) y += w(i)*fprime((x-a(i))/b(i))/b(i); // let u = (x-a)/b, then df(u)/dx = df(u)/du*du/dx, where du/dx = 1/b
		return y;
	}
	public double SecondDerivative(double x)
	{
		double y = 0;
		for(int i=0;i<n;i++) y += w(i)*fprimeprime((x-a(i))/b(i))/b(i)/b(i); // let u = (x-a)/b, then d²f(u)/dx = d/dx(df(u)/dx*du/dx) = d²f(u)/dx²*du/dx*du/dx + df(u)/dx*d²u/dx², where du/dx = 1/b and d²u/dx² = 0
		return y;
	}
	public double AntiDerivative(double x)
	{
		double y = 0;
		for(int i=0;i<n;i++) y += w(i)*F((x-a(i))/b(i))*b(i); // let u = (x-a)/b, then du/dx = 1/b and int f(u) dx = int f(u)*b*du 
		return y;
	}
	public void train(vector x, vector y)
	{
		Func<vector,double> cost = p =>	
		{
			this.p = p;
			double c = 0;
			for(int i=0;i<x.size;i++) c += Pow(response(x[i])-y[i],2);
			return c;
		};
		vector guess = new vector (3*n);
		for(int i=0;i<n;i++) {guess[i] = (double)(i+1)/n; guess[n+i] = 1; guess[2*n+i] = 1;}
		p = amoeba(cost,guess, acc: 1e-1, initialSize: 0.5, maxIterations: 20000).Item1;
		p = qnewton(cost,p, acc: acc, maxIterations: 10000);
	}
}
