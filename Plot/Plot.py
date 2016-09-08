import numpy as np
import matplotlib.pyplot as plt
import sys
import os

def main(argv):

	data = np.genfromtxt("centers.csv", delimiter = ',')
	
	for i in range(data.shape[0]):
		outputPath = "centerslearn/" + str(i) + ".png"
		plt.plot(data[i])
		plt.savefig(outputPath)
		plt.clf()
		print "Plot %d done" % (i)


if __name__ == '__main__':
    main(sys.argv[1:])