Out.txt : myMath.exe
	mono myMath.exe > Out.txt

sfuns.dll : static_sfuns.cs
	mcs -target:library -out:sfuns.dll static_sfuns.cs

myMath.exe : myMath.cs sfuns.dll
	mcs -target:exe -reference:sfuns.dll -out:myMath.exe myMath.cs

#myMath1.exe : myMath.cs static_sfuns.cs
#	mcs myMath.cs static_sfuns.cs -out:myMath1.exe

#sfuns.dll : static_sfuns.cs
#	mcs $^ -target:library -out:sfuns.dll

#myMath.exe : myMath.cs sfuns.dll
#	mcs myMath.cs -reference:sfuns.dll -out:myMath.exe

