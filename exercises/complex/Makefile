DIR = ../../matlib/complex

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs cmath.dll
	mcs -reference:cmath.dll main.cs

cmath.dll : $(DIR)/cmath.cs $(DIR)/complex.cs
	mcs -target:library -out:./cmath.dll $^

clean :
	rm *.txt *.dll *.exe
