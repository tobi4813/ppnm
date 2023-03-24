using System;
using static System.Console;
using static System.Math;

public class main
{
	public static void Main(string[] args)
	{
		double rmax = 10, dr = 0.3;
		bool rmaxFixed = false, drFixed = false, function = false, testEVD = false;
		int testSize = 1, numberOfFunctions = 1;

		foreach(var arg in args)
		{
			var words = arg.Split(":");
			if (words[0] == "-rmax") rmax = double.Parse(words[1]);
			else if (words[0] == "-dr") dr = double.Parse(words[1]);
			else if (words[0] == "-fixed")
			{
				if (words[1] == "rmax") rmaxFixed = true;
				else if (words[1] == "dr") drFixed = true;
			}
			else if (words[0] == "-test") {testEVD = true; testSize = int.Parse(words[1]);}
			else if (words[0] == "-functions") {function = true; numberOfFunctions = int.Parse(words[1]);}
		}

		if(rmaxFixed)
		{
			double drMin = 0.1, drMax = rmax/2;
			int resolution = 100;
			string title = "fixed R_{max}";
			WriteLine($"\"{title} = {rmax}\"");
			for(int i=0;i<resolution+1;i++)
			{
				double newDr = drMin + (drMax-drMin)/resolution*i;
				EVD burger = generateH(rmax, newDr);
				WriteLine($"{newDr} {burger.eigenvalues[0]}");
			}
		}
		else if (drFixed)
		{
			double rmaxMin = 0.1, rmaxMax = 5;
			if (dr >= rmaxMin) rmaxMin = 2*dr;
			int resolution = 100;
			string title = "fixed {/Symbol D}r"; //fixed \Delta r in gnuplot
			WriteLine($"\"{title} = {dr}\"");
			for(int i=0;i<resolution+1;i++)
			{
				double newRmax = rmaxMin + (rmaxMax-rmaxMin)/resolution*i;
				EVD burger = generateH(newRmax, dr);
				WriteLine($"{newRmax} {burger.eigenvalues[0]}");
			}
		}
		if(testEVD) TestEVD(testSize);
		if(function)
		{
			EVD burger = generateH(rmax, dr);
			int npoints = (int)(rmax/dr);
			for(int i=0;i<numberOfFunctions;i++)
			{
				WriteLine($"\"{i}'th-eigenfunction (numerical)\"");
				for(int j=0;j<npoints;j++)
				{
					double r_j = dr*(j+1); // r_j
					double fr_j = burger.V[j, i]*burger.V[j, i]; // i'th eigenfunciton at r_j
					WriteLine($"{r_j} {fr_j}");
				}
				Write("\n\n"); // 2 empty lines seperates datasets in gnuplot

				WriteLine($"\"{i}'th-eigenfunction (analytical)\"");
				int analyticalResolution = 1000;
				for(int j=0;j<analyticalResolution;j++)
				{
					double r = rmax/analyticalResolution*j;
					WriteLine($"{r} {f(r, i+1)/5}");
				}	
				if(i < numberOfFunctions-1) Write("\n\n");	
			}
		}
	}
	public static double f(double r, int n)
	{	
		switch(n)
		{
			case 1: return Pow(r*2*Exp(-r),(double)2); // r*R_10
			case 2: return Pow(r/Sqrt(2)*(1-r/2)*Exp(-r/2),(double)2); // r*R_20
			case 3: return Pow(r*2/(3*Sqrt(3))*(1-2*r/3 + 2*r*r/27)*Exp(-r/3),(double)2); // r*R_30
			default: return -1;
		}
	}
	public static EVD generateH(double rmax, double dr)
	{
		int npoints = (int)(rmax/dr);
		vector r = new vector(npoints);
		matrix H = new matrix(npoints,npoints);
		for(int i=0;i<npoints;i++) r[i] = dr*(i+1);
		for(int i=0;i<npoints-1;i++)
		{
			H[i,i] = -2;
			H[i,i+1] = 1;
			H[i+1,i] = 1;
		}
		H[npoints-1,npoints-1] = -2;
		matrix.scale(H,-0.5/dr/dr);
		for(int i=0;i<npoints;i++) H[i,i] -= 1/r[i];

		return new EVD(H);
	
	}
	public static void TestEVD(int n)
	{
		string divider = new string('-', n*11);
		matrix A = new matrix(n,n);
		A.randomizeSymmetric();

		WriteLine(divider);
		A.print("Random symmetrix matrix, A:");
		WriteLine(divider);

		EVD bob = new EVD(A);
		
		bob.V.print("Eigenvectors of A:");
		WriteLine(divider);
		
		bob.eigenvalues.print("Eigenvalues of A:\n");
		WriteLine(divider);

		matrix ATimesEigenvectors = new matrix(n,n);
		matrix eigenvaluesTimesEigenvectors = new matrix(n,n);	
		for(int i=0;i<n;i++){ATimesEigenvectors[i] = A*bob.V[i]; eigenvaluesTimesEigenvectors[i] = bob.eigenvalues[i]*bob.V[i];}		

		ATimesEigenvectors.print("Vectors produced from A*eigenvector (columns are the vectors):");
		WriteLine(divider); 
		
		eigenvaluesTimesEigenvectors.print("Vectors produced from eigenvalue*eigenvector (columns are the vectors):"); 
		WriteLine(divider);
		
		bob.D.print("Diagonalized A:");
		WriteLine(divider);

		matrix D = bob.V.transpose()*A*bob.V;
		D.print("Product VT * A * V:");
		WriteLine(divider);

		matrix newA = bob.V*bob.D*bob.V.transpose();
		newA.print("Product V D VT: ");
		WriteLine(divider);

		matrix I = bob.V.transpose()*bob.V;
		I.print("Product VT * V:");
		WriteLine(divider);
		
		I = bob.V*bob.V.transpose();
		I.print("Product V * VT:");
		WriteLine(divider);
	}
	
}
