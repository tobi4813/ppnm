DIR = ../../matlib/matrix

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll EVD.dll
	mcs $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

linalg.dll : $(DIR)/matrix.cs $(DIR)/vector.cs #$(DIR)/randomMatrix.cs
	mcs -target:library -out:./$@ $^

EVD.dll : EVD.cs
	mcs -target:library -out:./$@ $^ -reference:linalg.dll

clean :
	rm *.exe *.dll Out*
