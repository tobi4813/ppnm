using System;
using static System.Console;
using static System.Math;

public class main
{
	public static void Main()
	{
		int n = 5;
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
