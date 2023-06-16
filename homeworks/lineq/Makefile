DIR = ../../matlib/matrix
MKEXE = mcs $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))
TIMESFILE = QRtimes.data
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

Out.txt : main.exe opcount.exe plot.gpi Makefile
	>$(TIMESFILE)
	for n in $$(seq 100 50 600); do \
		time --format "$$n %e" --output $(TIMESFILE) --append \
		mono opcount.exe -N:$$n ;\
	done
	mono main.exe > Out.txt
	gnuplot plot.gpi

main.exe : main.cs linalg.dll QRGS.dll ppnm.dll
	$(MKEXE)

opcount.exe : opcount.cs linalg.dll QRGS.dll
	$(MKEXE)

linalg.dll : $(DIR)/matrix.cs $(DIR)/vector.cs
	$(MKLIB)

QRGS.dll : QRGS.cs linalg.dll
	$(MKLIB)

ppnm.dll : ../minimization/mini.cs ../../exercises/genlist/genlist.cs linalg.dll
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt *.data *.svg
