N = 5e7
TIME = time --portability --append --output $@
Out.txt : main.exe parallel.exe Makefile
	>$@	
	$(TIME) mono parallel.exe >>$@
	$(TIME) mono $< -terms:$N -threads:1 >>$@
	$(TIME) mono $< -terms:$N -threads:2 >>$@
	$(TIME) mono $< -terms:$N -threads:3 >>$@
	$(TIME) mono $< -terms:$N -threads:4 >>$@

main.exe : main.cs
	mcs main.cs

parallel.exe : parallel.cs
	mcs $<
