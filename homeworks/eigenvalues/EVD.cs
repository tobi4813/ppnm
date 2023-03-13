using static System.Math;
public class EVD
{
	public vector eigenvalues;
	public matrix V;
	public matrix D;
	
	public EVD(matrix A)
	{
		D = A.copy();
		V = matrix.id(D.size1);
		eigenvalues = new vector(D.size1);
		sweep();
	}

	static void timesJ(matrix A, int p, int q, double theta)
	{
		double c = Cos(theta), s = Sin(theta);
		for(int i=0;i<A.size1;i++)
		{
			double Aip = A[i,p], Aiq = A[i,q]; 
			A[i,p] = c*Aip - s*Aiq;
			A[i,q] = s*Aip + c*Aiq;
		}
	}

	static void Jtimes(matrix A, int p, int q, double theta)
	{
		double c = Cos(theta), s = Sin(theta);
		for(int i=0;i<A.size1;i++)
		{
			double Api = A[p,i], Aqi = A[q,i]; 
			A[p,i] =  c*Api + s*Aqi;
			A[q,i] = -s*Api + c*Aqi;
		}
	}

	public void sweep()
	{
		bool changed;
		int sweeps = 0;
		do
		{
			sweeps++;
			changed = false;
			for(int p=0;p<D.size1-1;p++)
				for(int q=p+1;q<D.size1;q++)
				{
					double Dpq = D[p,q], Dpp = D[p,p], Dqq = D[q,q];
					double theta = 0.5*Atan2(2*Dpq, Dqq-Dpp);
					double c = Cos(theta), s = Sin(theta);
					double newDpp = c*c*Dpp - 2*s*c*Dpq + s*s*Dqq;
					double newDqq = s*s*Dpp + 2*s*c*Dpq + c*c*Dqq;
					if(Abs(newDpp-Dpp) > 1e-9 || Abs(newDqq - Dqq) > 1e-9)
						{
							changed = true;
							timesJ(D,p,q, theta);
							Jtimes(D,p,q,-theta); //J^T (theta) = J(-theta)
							timesJ(V,p,q, theta);
						}
				}
		}
		while(changed && sweeps < 1e5); 

		for(int i=0;i<D.size1;i++) eigenvalues[i] = D[i,i];
	}
	
}
