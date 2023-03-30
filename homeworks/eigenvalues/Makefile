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

fixedRmax.data : main.exe Makefile
	mono $< -rmax:50 -fixed:rmax > $@

fixedDr.data : main.exe Makefile
	mono $< -dr:0.2 -fixed:dr > $@

waveFunction.data : main.exe Makefile
	mono $< -rmax:30 -dr:0.2 -functions:3 > $@ # rmax needs to be large enough for E1 and E2 to converge with dr = 0.2

#out.data out1.data out2.data : main.exe
#	mono $< 1 > out.data 2 > out2.data ... #$>@ refers to target that was triggered

clean :
	rm *.exe *.dll Out* *.data *.svg
