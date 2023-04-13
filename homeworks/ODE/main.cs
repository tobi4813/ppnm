using System;
using System.IO;
using static System.Console;
using static solveODE;
using static System.Math;
using System.Collections.Generic;

public class main
{
	static List<double> masses;
	static double G;
	static double eps;
	
	public static void Main()
	{
		Tests();
		Eight();
		PlanetaryMotion();
	}
	static void Tests()
	{
		genlist<double> xs;
		genlist<vector> ys;
		
		(xs,ys) = drive(FirstOrder, x0: 0, y0: new vector("1"), xf: 2, h: 1e-2, acc: 1e-7, eps: 1e-7);
		WriteData(xs, ys, "\"numerical, y' = y. y0 = 1 => y = exp\"",outfile: "y'=y.data", analytical: Exp);	
		
		(xs,ys) = drive(SecondOrder, x0: 0, y0: new vector("1 0"), xf: 3*PI, h: 1e-2, acc: 1e-7, eps: 1e-7); //y0 = [y0, y'0]
		WriteData(xs, ys, "\"numerical, y'' = -y. y0 = 1, y'0 = 0 => y = cos\"",outfile: "y''=-y.data", analytical: Cos);
		
		(xs,ys) = drive(Pendulum, x0: 0, y0: new vector($"{PI-0.1} 0"), xf: 10, h: 5e-1, acc: 1e-7, eps: 1e-7); //y0 = [y0, y'0]
		WriteData(xs, ys, "t \"{/Symbol q}(t)\" \"{/Symbol w}(t)\"",outfile: "pendulum.data");

		genlist<double> xlist = new genlist<double>();
		genlist<vector> ylist = new genlist<vector>();
		vector y = driver(SecondOrder, x0: 0, y0: new vector("1 0"), xf: 3*PI, h: 1e-2, acc: 1e-7, eps: 1e-7); //
		WriteLine($"Solving u'' = -u while only keeping final point: size of xlist={xlist.size}, size of ylist={ylist.size}, final point={y[0]}=cos(3*pi)={Cos(3*PI)}");
		driver(SecondOrder, x0: 0, y0: new vector("0 1"), xf: 3*PI, h: 1e-2, acc: 1e-7, eps: 1e-7, xlist: xlist, ylist: ylist); //
		WriteLine($"After feeding the driver empty lists: size of xlist={xlist.size}, size of ylist={ylist.size}");
		WriteData(xlist,ylist,"\"numerical, y'' = -y. y0 = 0, y'0 = 0 => y = sin\"",outfile: "B y''=-y.data", analytical: Sin);
	}	
	
	static void WriteData(genlist<double> xdata, genlist<vector> ydata, string name, string outfile, Func<double,double> analytical=null)
	{
		using (StreamWriter output = new StreamWriter($"data/{outfile}", append: false))
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
	{	//theta'' + b*theta' + c*sin(theta) = 0
		double theta = y[0], thetaP = y[1];
		double b = 0.25, c = 5;
		double thetaPP = -b*thetaP - c*Sin(theta);
		return new vector(thetaP, thetaPP);
	}

	static void Eight()
	{	//y0 = x1_0 y1_0 x1'_0 y1' x2_0 y2_0 x2'_0 ... xi_0 yi_0
		masses = new List<double>() {1,1,1};
		G = 1;
		genlist<double> xs;
		genlist<vector> ys;
		double x1=0.97000436, y1=-0.24308753,x2=-x1, y2=-y1, x3=0, y3=0;
		double vx3=-0.93240737, vy3=-0.86473146, vx1=-vx3/2, vy1=-vy3/2, vx2=vx1, vy2=vy1;
		vector y0 = new vector($"{x1} {y1} {vx1} {vy1} {x2} {y2} {vx2} {vy2} {x3} {y3} {vx3} {vy3}");

		(xs,ys) = drive(Gravitation, 0, y0, 6.32591398, acc: 1e-6, eps: 1e-6,method: "rkf45"); //1e-8 1e-9
		WriteData(xs, ys, "t x1 m_1 vx1 vy1 x2 m_2 vx2 vy2 x3 m_3 vx3 vy3", "eight.data");
	}
	
	static void PlanetaryMotion()
	{
		// test of newtonian gravitation
		masses = new List<double>(){2e30, 6e24};
		G = 6.67e-11;
		genlist<double> xs;
		genlist<vector> ys;
		double sunX=0, sunY=0, sunVX=0, sunVY=0, earthX=149577000000, earthY=0, earthVX=0, earthVY=29780; //units [r] = m, [v] = m/s 
		vector y0 = new vector($"{sunX} {sunY} {sunVX} {sunVY} {earthX} {earthY} {earthVX} {earthVY}");

		(xs,ys) = drive(Gravitation, x0: 0, y0: y0, xf: 3.16e7, h: 1e-2, acc: 1e-7, eps: 1e-7); 
		WriteData(xdata: xs, ydata: ys, name: "time sunx Sun sunvx sunvy earthx Earth earthvx earthvy", outfile: "planetHighAcc.data");

		(xs,ys) = drive(Gravitation, x0: 0, y0: y0, xf: 1e9, h: 1e-2, acc: 1e-2, eps: 1e-2, method: "rkf45");
		WriteData(xdata: xs, ydata: ys, name: "time sunx Sun sunvx sunvy earthx Earth earthvx earthvy", outfile: "planetLowAcc.data");

		// B) relativistic motion
		xs = new genlist<double>(); 
		ys = new genlist<vector>();
		y0 = new vector(1, 0);

		eps = 0;
		driver(relativisticMotion, x0: 0, y0: y0, xf: 1*2*PI, h: 1e-2, acc: 1e-7, eps: 1e-7, xlist: xs, ylist: ys, method: "rkf45");
		WriteData(xs, ys, "a planet", outfile: "GRi.data");

		y0 = new vector(1, -0.5);
		(xs,ys) = drive(relativisticMotion, x0: 0, y0: y0, xf: 1*2*PI, h: 1e-2, acc: 1e-7, eps: 1e-7, method: "rkf45");
		WriteData(xs, ys, "a planet", outfile: "GRii.data");

		eps = 0.01;
		(xs,ys) = drive(relativisticMotion, x0: 0, y0: y0, xf: 4*2*PI, h: 1e-2, acc: 1e-7, eps: 1e-7, method: "rkf45");
		WriteData(xs, ys, "a planet", outfile: "GRiii.data");

		(xs,ys) = drive(relativisticMotion, x0: 0, y0: y0, xf: 98*2*PI, h: 1e-2, acc: 1e-6, eps: 1e-6, method: "rkf45");
		WriteData(xs, ys, "a planet", outfile: "donut.data");
			
	}
	static vector relativisticMotion(double phi, vector y)
	{
		double u = y[0], uP = y[1];  //u, u'
		double uPP = 1 - u + eps*u*u; //u''
		vector dydphi = new vector(uP,uPP);
		return dydphi;
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
