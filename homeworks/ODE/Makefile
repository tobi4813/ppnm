# small libraries (like EVD.dll) which we need to recompile often should be recompiled seperately (not together with eg. linalg.dll 
# which doesnt really change
LINALGDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

$(info Making the data may take up to ~30s depending on hardware)

plots.svg : plots.gpi Out.txt main.cs
	gnuplot $<

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs linalg.dll genlist.dll ode.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(LINALGDIR)/matrix.cs $(LINALGDIR)/vector.cs
	$(MKLIB)

genlist.dll : $(GENLISTDIR)/genlist.cs
	$(MKLIB)

ode.dll : ode.cs linalg.dll genlist.dll
	$(MKLIB)

clean :
	rm *.exe *.dll Out* *.svg *.data
