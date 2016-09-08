import numpy as np
import sys

def norm_row(row):
	#t = row - np.min(row)
	#return t/np.average(np.sort(t)[-5:])
	return row - np.average(row)

def main(argv):
	data = np.genfromtxt("learn.csv", delimiter = ',')
	data = np.apply_along_axis(norm_row, 1, data)
	print data
	np.savetxt("learn_ready.csv", data, delimiter=",")

if __name__ == '__main__':
	main(sys.argv[1:])