DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

Out.txt : main.exe pythonCounts.txt Makefile
	python3 integrate.py > pythonCounts.txt
	mono main.exe > Out.txt

main.exe : main.cs integrate.dll
	mcs $(CODE) $(DLLS)

pythonCounts.txt : integrate.py

integrate.dll : integrate.cs
	$(MKLIB)

clean :
	rm *.exe *.dll *.txt
