MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plot : plot.gpi Out.txt
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll mini.dll ann.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

mini.dll : ../minimization/mini.cs linalg.dll
	$(MKLIB)

ann.dll : ann.cs mini.dll linalg.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.svg \
	rm -rfv data
