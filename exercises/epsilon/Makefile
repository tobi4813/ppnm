Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs epsilon.dll
	mcs main.cs -reference:epsilon.dll -target:exe -out:main.exe

epsilon.dll : epsilon.cs
	mcs $< -target:library -out:$@
