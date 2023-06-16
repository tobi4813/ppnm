using System;

static class opCount
{
	static void Main(string[] args)
	{
		int N = 0;
		foreach(var arg in args)
		{
			var words = arg.Split(":");
			if (words[0] == "-N") N = int.Parse(words[1]);
		}
		Random rnd = new Random();
		matrix randomMatrix = new matrix(N,N);
		for(int i=0;i<N;i++) for(int j=0;j<N;j++) randomMatrix[i,j] = rnd.NextDouble();
		new QRGS(randomMatrix, copy: false);	
	}
}
