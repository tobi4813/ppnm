MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plot : plots.gpi Out.txt
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll mini.dll mc.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

mini.dll : ../minimization/mini.cs linalg.dll
	$(MKLIB)

mc.dll : mc.cs linalg.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.svg
	rm -rfv data
