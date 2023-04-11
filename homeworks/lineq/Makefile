DIR = ../../matlib/matrix

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll QRGS.dll
	mcs $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

linalg.dll : $(DIR)/matrix.cs $(DIR)/vector.cs
	mcs -target:library -out:./$@ $^

QRGS.dll : QRGS.cs
	mcs -target:library -out:./$@ $^ -reference:linalg.dll

clean :
	rm *.exe *.dll Out*
