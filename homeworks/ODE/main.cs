using System;
using System.IO;
using static System.Console;
using static solveODE;
using static System.Math;

public class main
{
	public static void Main()
	{
		genlist<double> xs;
		genlist<vector> ys;
		
		(xs,ys) = drive(FirstOrder, x0: 0, y0: new vector("1"), xf: 2, h: 1e-2, acc: 1e-8, eps: 1e-9);
		WriteData(xs, ys, "\"numerical, y' = y. y0 = 1 => y = exp\"",outfile: "y'=y.data", analytical: Exp);	
		
		(xs,ys) = drive(SecondOrder, x0: 0, y0: new vector("1 0"), xf: 3*PI, h: 1e-2, acc: 1e-8, eps: 1e-9); //y0 = [y0, y'0]
		WriteData(xs, ys, "\"numerical, y'' = -y. y0 = 1, y'0 = 0 => y = cos\"",outfile: "y''=-y.data", analytical: Cos);
		
		(xs,ys) = drive(Pendulum, x0: 0, y0: new vector($"{PI-0.1} 0"), xf: 10, h: 5e-1, acc: 1e-8, eps: 1e-8); //y0 = [y0, y'0]
		WriteData(xs, ys, "t \"{/Symbol q}(t)\" \"{/Symbol w}(t)\"",outfile: "pendulum.data");
	}
	static void WriteData(genlist<double> xdata, genlist<vector> ydata, string name, string outfile, Func<double,double> analytical=null)
	{
		var output = new StreamWriter(outfile, append: false);
		output.WriteLine(name); // numerical-data header
		for(int i=0;i<xdata.size;i++) 
		{
			string outfile_content = $"{xdata[i]}";
			for(int j=0;j<ydata[0].size;j++) outfile_content += $" {ydata[i][j]}";
			output.WriteLine(outfile_content);
		}
		if(analytical != null)
		{
			output.Write("\n\n\"Analytical solution\"\n"); // analytical-data header
			for(int i=0;i<xdata.size;i++) output.WriteLine($"{xdata[i]} {analytical(xdata[i])}");	
		}
	}
	
	static vector FirstOrder(double x, vector y) {return y;}//dydx = y
	static vector SecondOrder(double x, vector u)
	{
		double y = u[0];
		double yP = u[1]; //yP = yprime
		double yPP = -y;
		return new vector(yP, yPP);
		
	}
	static vector Pendulum(double x, vector y)
	{
		//theta'' + b*theta' + c*sin(theta) = 0
		double theta = y[0], thetaP = y[1];
		double b = 0.25, c = 5;
		double thetaPP = -b*thetaP - c*Sin(theta);
		return new vector(thetaP, thetaPP);
	}
}
