using System;
using System.IO;
using static System.Console;
using static System.Math;
using static Minimization;

public class main
{
	public static void Main()
	{
		//tests();
		fit();
	}
	public static void tests()
	{
		int n = 10, m = 6;
		matrix A = RandomMatrix(n,m);
	
		string divider = new string('-',m*11);
		WriteLine(divider);
		A.print($"Random {n}x{m} matrix, A, below:");
		WriteLine(divider);
		
		QRGS yolo = new QRGS(A);
		yolo.R.print("R matix below:");
		WriteLine(divider);
	
		matrix I = yolo.Q.transpose()*yolo.Q;
		I.print("Hopefully the identity matrix below (QT*Q):");
		WriteLine(divider);
		
		matrix QR = yolo.Q*yolo.R;
		QR.print("Q*R matrix below");
		WriteLine(divider);
		
		A = RandomMatrix(m,m);
		vector b = RandomVector(m);
		QRGS yo = new QRGS(A);
		
		A.print($"Random square {m}x{m} matrix, A, below: ");
		WriteLine(divider);
		
		b.print($"Random {m} element vector b:\n");
		WriteLine(divider);
		
		vector x = yo.solve(b);	
		x.print("Vector x, solving Ax = b:\n");
		WriteLine(divider);
		
		vector prod = A*x;	
		prod.print("A * x:\n");
		WriteLine(divider);
		
		WriteLine($"det(A) = {yo.det()}");
		WriteLine(divider);

		matrix inverseA = yo.inverse();
		inverseA.print("Inverse A matrix below:");
		WriteLine(divider);

		I = A*inverseA;
		I.print("A*A⁻¹ below:");
		WriteLine(divider);
	}
	public static matrix RandomMatrix(int n, int m)
	{
		var rnd = new Random(171000);
		matrix randomMatrix = new matrix(n,m);
		for(int i=0;i<n;i++) for(int j=0;j<m;j++) randomMatrix[i,j] = rnd.NextDouble();
		return randomMatrix;
	}	
	public static vector RandomVector(int n)
	{
		var rnd = new Random(201907250);
		vector randomVector = new vector(n);
		for(int i=0;i<n;i++) randomVector[i] = rnd.NextDouble();
		return randomVector;
	}
	public static void fit()
	{
		Func<vector,double,double> polynomium = (parameters,x) => parameters[0]*x*x*x;
		
		var data = new StreamReader("QRtimes.data");
		genlist<int> Ns = new genlist<int>();
		genlist<double> ts = new genlist<double>();
		for(string line=data.ReadLine(); line!=null; line=data.ReadLine()) 
		{
			var words = line.Split(' ');
			Ns.add(int.Parse(words[0]));
			ts.add(double.Parse(words[1]));
		}
		
		Func<vector,double> Deviation = v =>
		{	
			double sum = 0;
			for(int i=0;i<ts.size;i++) sum+=Pow(polynomium(v,Ns[i]) - ts[i], 2);
			return sum; 
		};
		vector guess = new vector(1e-9);
		vector optimalParameters = amoeba(Deviation, guess, acc: 1e-11).Item1;
		WriteLine($"The time it takes goes as {optimalParameters[0]:e2} N³");
		int resolution = 100;
		using(var output = new StreamWriter("fit.data"))
		{
			for(int i=0;i<resolution;i++)
			{
				double N = Ns[0] + ((double) Ns[Ns.size-1]-Ns[0])/(resolution - 1) * i;
				double t = polynomium(optimalParameters,N);
				output.WriteLine($"{N} {t}");
			}
		}
		data.Close();
	}
}
