using System;
using System.IO;
using static System.Console;
using static solveODE;
using static System.Math;

public class main
{
	public static double[] masses = {2e30, 6e24};
	public static double G = 6.67e-11;
	
	public static void Main()
	{
		Tests();
		PlanetaryMotion();
	}
	static void Tests()
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
		using (StreamWriter output = new StreamWriter(outfile, append: false))
		{
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
	
	static void PlanetaryMotion()
	{
		genlist<double> xs;
		genlist<vector> ys;
		//y0 = x1_0 y1_0 x1'_0 y1' x2_0 y2_0 x2'_0 ... xi_0 yi_0
		double sunX=0, sunY=0, sunVX=0, sunVY=0, earthX=149577000000, earthY=0, earthVX=0, earthVY=29780; //[r] = m, [v] = m/s 
		vector y0 = new vector($"{sunX} {sunY} {sunVX} {sunVY} {earthX} {earthY} {earthVX} {earthVY}");
		(xs,ys) = drive(Gravitation, x0: 0, y0: y0, xf: 3.3e7, h: 1e-2, acc: 1e-8, eps: 3e-9); //eps=3e-9 pretty accurate ~10 seconds run time
		WriteData(xdata: xs, ydata: ys, name: "time sunx Sun sunvx sunvy earthx Earth earthvx earthvy", outfile: "planetHighAcc.data");

		(xs,ys) = drive(Gravitation, x0: 0, y0: y0, xf: 3.3e7, h: 1e-2, acc: 1e-7, eps: 5e-8); //eps=3e-9 pretty accurate ~10 seconds run time
		WriteData(xdata: xs, ydata: ys, name: "time sunx Sun sunvx sunvy earthx Earth earthvx earthvy", outfile: "planetLowAcc.data");	
	}
	
	static vector Gravitation(double t, vector y)
	{ 	//y = x1_0 y1_0 x1'_0 y1' x2_0 y2_0 x2'_0 ... xi_0 yi_0
		vector dydt = new vector(y.size);
		for(int i=0;i<y.size/4;i++) // y always multiple of 4 since one planet adds 4 elements to y (x0,y0,x'0,y'0)
		{
			double mi = main.masses[i];
			vector ri = new vector(y[4*i], y[4*i+1]);
			vector dridt = new vector(y[4*i+2], y[4*i+3]);
			vector dvidt = new vector(2);
			for(int j=0;j<y.size/4;j++)
				if(j!=i)
				{
					double mj = masses[j];
					vector rj = new vector(y[4*j], y[4*j+1]);
					vector rdiff = rj-ri;
					dvidt += G*mi*mj/Pow(rdiff.norm(), 3) * rdiff/mi;
				}
			dydt[4*i] = dridt[0];	//x'  = dxdt
			dydt[4*i+1] = dridt[1];	//y'  = dydt
			dydt[4*i+2] = dvidt[0]; //x'' = dvxdt
			dydt[4*i+3] = dvidt[1]; //y'' = dvydt
		}
		return dydt;
	}
}
