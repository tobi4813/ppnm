 MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
TIME = time --portability --append --output $@


plot : hydrogen.gpi Out.txt #log.dat
	gnuplot $<

Out.txt : main.exe
	#rm -f test.txt
	$(TIME) mono main.exe > Out.txt

main.exe : main.cs roots.dll linalg.dll ppnm.dll # QRGS.dll ode.dll genlist.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

ppnm.dll : ../lineq/QRGS.cs $(GENLISTDIR)/genlist.cs ../ODE/ode.cs linalg.dll
	$(MKLIB)

roots.dll : roots.cs linalg.dll ppnm.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.svg
	rm -rfv data
