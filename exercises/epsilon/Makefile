Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs
	mcs main.cs -target:exe -out:main.exe

clean : 
	rm *.dll *.exe *.txt
