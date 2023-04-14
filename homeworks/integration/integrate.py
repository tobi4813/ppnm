import numpy as np
from scipy.integrate import quad
import math
def test2(x):
	return 1/math.sqrt(x)

def test4(x):
	return math.log(x)/math.sqrt(x)

# scipy.integrate.quad returns a dictionary as its 3 element when full_output = 1. It contains the number of evaluations neval
integral = quad(test2, 0, 1,full_output=1, epsabs=1e-3, epsrel=1e-3)
print((integral[0], integral[2]["neval"]))

integral = quad(test4, 0, 1,full_output=1, epsabs=1e-3, epsrel=1e-3)
print((integral[0],integral[2]["neval"]))
