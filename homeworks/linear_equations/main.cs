using System;
using static System.Console;
using static System.Math;

public class main
{
	public static void Main()
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
}
