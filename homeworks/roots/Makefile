MATLIBDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

Out.txt : main.exe
	rm -f test.txt
	mono main.exe > Out.txt

main.exe : main.cs roots.dll linalg.dll QRGS.dll ode.dll genlist.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(MATLIBDIR)/matrix.cs $(MATLIBDIR)/vector.cs
	$(MKLIB)

QRGS.dll : ../lineq/QRGS.cs linalg.dll
	$(MKLIB)

roots.dll : roots.cs linalg.dll QRGS.dll
	$(MKLIB)

genlist.dll : $(GENLISTDIR)/genlist.cs
	$(MKLIB)

ode.dll : ../ODE/ode.cs linalg.dll genlist.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt
