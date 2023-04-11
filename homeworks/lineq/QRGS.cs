public class QRGS
{
	public matrix Q, R;
	int m;
	public QRGS(matrix A)
	{
		m = A.size2;
		Q = A.copy(); // Q = A, if we want to overwrite A as we make Q
		R = new matrix(m,m);
		QRDecomp(Q, R);
	}
	
	public void QRDecomp(matrix Q, matrix R)
	{
		for(int i=0; i<m; i++)
		{
			R[i,i] = Q[i].norm();
			Q[i] /= R[i,i];
			for(int j=i+1; j<m; j++)
			{
				R[i,j] = Q[i].dot(Q[j]);
				Q[j] -= Q[i]*R[i,j];
			}
		}
	}
	
	public vector solve(vector b)
	{
		//b = Q.transpose()*b; // if we don't want to create vector c
		vector c = Q.transpose()*b;
		vector x = new vector(m);
		for(int i=m-1;i>=0;i--)
		{
			double sum = c[i];
			for(int j=i+1;j<m;j++) sum -= R[i,j]*x[j];
			x[i] = sum/R[i,i]; // x_i = (b_i - R_i,i+1 * x_i+1 - R_i,i+2 * x_i+2 - ... - R_i,n * x_n)/R_i,i 
		}	
		return x;
	}
	
	public double det()
	{
		double determinant = 0;
		for(int i=0;i<m;i++) determinant+=R[i,i];
		return determinant;
	}
	
	public matrix inverse()
	{
		matrix inverseA = new matrix(m,m);
		for(int i=0;i<m;i++)
		{
			vector unitVector = new vector(m);
			for(int j=0;j<m;j++) if(j == i) unitVector[j] = 1;
			inverseA[i] = this.solve(unitVector);
		}
		return inverseA;
	}
}
