MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll mini.dll # ppnm.dll # QRGS.dll ode.dll genlist.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

mini.dll : mini.cs linalg.dll
	$(MKLIB)

#ppnm.dll : ../lineq/QRGS.cs $(GENLISTDIR)/genlist.cs ../ODE/ode.cs linalg.dll
#	$(MKLIB)

clean :
	rm *.exe *.dll *.txt
