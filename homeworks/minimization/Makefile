MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plot : higgs.gpi Out.txt
	gnuplot $<

Out.txt : main.exe
	mono main.exe <higgs.dat 1> Out.txt

main.exe : main.cs linalg.dll mini.dll ppnm.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

mini.dll : mini.cs linalg.dll
	$(MKLIB)

ppnm.dll : $(GENLISTDIR)/genlist.cs 
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.data *.svg
