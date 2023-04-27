MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plot : plot.gpi Out.txt
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll splines.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

splines.dll : splines.cs linalg.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.data *.svg
