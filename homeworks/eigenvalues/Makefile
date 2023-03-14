DIR = ../../matlib/matrix

Out.txt : main.exe swave.svg Makefile
	mono main.exe -test:5 > Out.txt

swave.svg : swave.gpi fixedRmax.data fixedDr.data waveFunction.data
	gnuplot $<

main.exe : main.cs linalg.dll EVD.dll
	mcs $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

linalg.dll : $(DIR)/matrix.cs $(DIR)/vector.cs #$(DIR)/randomMatrix.cs
	mcs -target:library -out:./$@ $^

EVD.dll : EVD.cs
	mcs -target:library -out:./$@ $^ -reference:linalg.dll

fixedRmax.data : main.exe
	mono $< -rmax:30 -fixed:rmax > $@

fixedDr.data : main.exe
	mono $< -dr:0.3 -fixed:dr > $@

waveFunction.data : main.exe Makefile
	mono $< -rmax:10 -dr:0.2 -functions:2 > $@

clean :
	rm *.exe *.dll Out* *.data *.svg
