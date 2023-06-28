MATLIBDIR = ../matlib/matrix
GENLISTDIR = ../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plot : plot.gpi Out.txt
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll genlist.dll splines.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

genlist.dll : $(GENLISTDIR)/genlist.cs
	$(MKLIB)

splines.dll : splines.cs linalg.dll genlist.dll
	$(MKLIB)


clean :
	rm *.exe *.dll *.txt *.svg
	rm -rfv data
