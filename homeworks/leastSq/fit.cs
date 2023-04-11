using System;

public static class Fit
{
	public static (vector,matrix) lsfit(Func<double,double>[] fs, vector x, vector y, vector dy)
	{
		int n = x.size, m = fs.Length;
		matrix A = new matrix(n,m);
		vector b = new vector(n);
		for(int i=0;i<n;i++)
		{
			for(int k=0;k<m;k++)
			{
				A[i,k] = fs[k](x[i])/dy[i];
				
			}
			b[i] = y[i]/dy[i];
		}
		QRGS QRfact = new QRGS(A);
		vector c = QRfact.solve(b);
		QRGS ATA = new QRGS(A.transpose()*A);
		matrix covariance = ATA.inverse();
		return (c,covariance);
	}
}
