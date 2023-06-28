infile = input.data
DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKEXE = mcs -target:exe -out:$@ $(DLLS) $(CODE)
MKDLL = mcs -target:library -out:$@ $(DLLS) $(CODE)

Out.txt : main.exe
	mono $< < $(infile) > Out.txt

main.exe : main.cs genlist.dll
	$(MKEXE)

genlist.dll : genlist.cs
	$(MKLIB)

clean:
	rm *.txt *.exe
