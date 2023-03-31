DLLS = $(addprefix -reference:,$(filter %.dll,$^))
CODE = $(filter %.cs,$^)
MKLIB = mcs -target:library -out:$@ $(DLLS) $(CODE)

Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs integrate.cs
	mcs $(CODE) $(DLLS)

integrate.dll : integrate.cs
	$(MKLIB)

clean :
	rm *.exe *.dll Out*
