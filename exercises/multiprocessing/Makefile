N = 1e9
TIME = time --portability --append --output $@
Out.txt : main.exe Makefile
	>$@
	$(TIME) mono $< -terms:$N -threads:5 >>$@
	$(TIME) mono $< -terms:$N -threads:6 >>$@
	$(TIME) mono $< -terms:$N -threads:7 >>$@
	$(TIME) mono $< -terms:$N -threads:8 >>$@

main.exe : main.cs
	mcs main.cs
