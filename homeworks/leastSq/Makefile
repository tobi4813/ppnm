MATLIBDIR = ../../matlib/matrix
QRDIR = ../lineq
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plots : plots.gpi Out.txt main.cs
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll QRGS.dll fit.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

QRGS.dll : $(QRDIR)/QRGS.cs linalg.dll
	$(MKLIB)

fit.dll : fit.cs linalg.dll QRGS.dll
	$(MKLIB)

clean :
	rm *.exe *.dll Out* *.svg *.data
