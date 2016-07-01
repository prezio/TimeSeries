import numpy as np
import sys
import os

def main(argv):

	data = np.genfromtxt("input.csv", delimiter = ',')
	results = np.zeros(data.shape)

	radius = 10
	for i in range(data.shape[0]):
		for j in range(len(data[i])):
			amount = 0
			for k in range(max(j-radius, 0), min(j+radius, len(data[i]))):
				results[i, j] += data[i, k]
				amount += 1
			results[i, j] /= amount

	np.savetxt("output.csv", results, delimiter=",")

if __name__ == '__main__':
    main(sys.argv[1:])