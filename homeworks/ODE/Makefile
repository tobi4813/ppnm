# small libraries (like EVD.dll) which we need to recompile often should be recompiled seperately (not together with eg. linalg.dll 
# which doesnt really change
LINALGDIR = ../../matlib/matrix
GENLISTDIR = ../../exercises/genlist
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)
TIME = time --portability --append --output Out.txt

#$(info Making the data may take up to ~1s depending on hardware)

plots : plots.gpi Out.txt main.cs ode.cs
	gnuplot $< 2>log

Out.txt : main.exe
	#$(TIME)\
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
	rm log *.exe *.dll Out* *.svg\
	rm -rfv data Plots
