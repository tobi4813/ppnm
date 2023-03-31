# small libraries (like EVD.dll) which we need to recompile often should be recompiled seperately (not together with eg. linalg.dll 
# which doesnt really change
LINALGDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

plots.svg : plots.gpi out.txt main.cs
	gnuplot $<
	
out.txt : main.exe # Only used for debugging
	mono main.exe > out.txt

main.exe : main.cs linalg.dll genlist.dll ode.dll
	mcs $(CODE) $(DLLS)

linalg.dll : $(LINALGDIR)/matrix.cs $(LINALGDIR)/vector.cs
	$(MKLIB)

genlist.dll : $(GENLISTDIR)/genlist.cs
	$(MKLIB)

ode.dll : ode.cs linalg.dll genlist.dll
	$(MKLIB)

clean :
	rm *.exe *.dll out* *.svg *.data