MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plot : plots.gpi Out.txt
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll ppnm.dll mc.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

ppnm.dll : ../minimization/mini.cs $(GENLISTDIR)/genlist.cs linalg.dll
	$(MKLIB)

mc.dll : mc.cs linalg.dll ppnm.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.svg
	rm -rfv data
