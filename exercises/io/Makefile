##myString = -numbers:1,2,3

Out.txt : main.exe main2.exe read.exe Makefile
	echo 1 2 3 4 5 | mono main2.exe 2> Out.txt
	echo "1\n2\n3\n4\n5" > input.txt
	mono read.exe -input:input.txt -output:output.txt

main.exe : main.cs
	mcs $(filter %.cs,$^)

main2.exe : main2.cs
	mcs $<

read.exe : read.cs
	mcs $<

clean :
	rm *.exe *.txt
