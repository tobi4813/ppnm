plots : gamma.gpi gamma.data factorials.data erf.data erfPoints.data
	gnuplot $<

gamma.data : main.exe
	mono $< "gamma" > $@

erf.data : main.exe
	mono $< "erf" > $@


main.exe : main.cs sfuns.dll
	mcs -target:exe -out:$@ $(filter %.cs,$^) $(addprefix -reference:,$(filter %.dll,$^))

sfuns.dll : gamma.cs erf.cs
	mcs -target:library -out:$@ $^

factorials.data : Makefile
	>$@
	echo 0 1 >>$@
	echo 1 1 >>$@
	echo 2 2 >>$@
	echo 3 6 >>$@
	echo 4 24 >>$@
	echo 5 120 >>$@
	#echo 6 "echo '6*120') | bc" >>$@

erfPoints.data : Makefile
	>$@
	echo 0 0 >>$@
	echo 0.06 0.068 >>$@
	echo 0.1 0.112 >>$@
	echo 0.3 0.329 >>$@
	echo 0.5 0.520 >>$@
	echo 0.8 0.742 >>$@
	echo 1.3 0.934 >>$@
	echo 1.9 0.993 >>$@
	echo 2.4 0.999 >>$@
