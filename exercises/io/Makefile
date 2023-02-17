##myString = -numbers:1,2,3

Out.txt : main.exe Makefile
	#echo "-numbers:1,2,3,4,5" > inputfile
	mono main.exe $(cat inputfile) > Out.txt


main.exe : main.cs
	mcs $(filter %.cs,$^)
	
