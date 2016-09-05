import csv
import time
from datetime import datetime
import numpy as np
import sys
	
def dtw(a, b):
	s1 = len(a)
	s2 = len(b)
	arr = np.zeros((s1+1, s2+1, 3))
	arr[1, 1] = [abs(a[0] - b[0]), 0, 0]
	for i in range(2, s1+1):
		arr[i, 1] = [arr[i-1, 1, 0] + abs(a[i-1] - b[0]), i-1, 1]
	for i in range(2, s2+1):
		arr[1, i] = [arr[1, i-1, 0] + abs(a[0] - b[i-1], 1, i-1]
	
	for i in range(2, s1+1):
		for j in range(2, s2+1):
			minimum = min([arr[i-1, j], arr[i, j-1], arr[i-1, j-1]])
			arr[i, j] = [abs(arr[i-1] - b[j-1]) + minimum[0], minimum[1], minimum[2]]
	return arr[s1, s2]

def k_means_alg(k, dist, data, it):
	size = data.shape[0]
	centers = np.random.permutation(size)[:k]
	centers.sort()
	centers = map(lambda x: data[x].copy(),centers)
	clusters = np.zeros(size)

	while it > 0:
		sums = np.zeros((k, data.shape[1]))
		amounts = np.zeros(k)
 
		for i in range(size):
			distances = map(lambda x: dist(data[i], x), centers)
			clust = distances.index(min(distances))
			clusters[i] = clust
			# for averaging
			sums[clust] += data[i]
			amounts[clust] += 1

		# recalculating centers
		centers = map(lambda i: sums[i] / amounts[i] if amounts[i] != 0 else centers[i], range(k))
		print '%d iterations left' % (it)
		it -= 1
	return clusters

def serialize(data, clusters, prec, iters, k):
	res = np.zeros((data.shape[0], data.shape[1]+1))
	res[:, :-1] = data
	res[:, -1] = clusters
	np.savetxt('results/result_k%d_prec%d_iters%d.csv' % (k, prec, iters), res, delimiter=",")

def main(argv):
	iters = 5
	k = 5
	filename = 'data.csv'	
	
	for arg in argv:
        	params = arg.split(':')
		if params[0] == '-f':
			filename = params[1]
		elif params[0] == '-i':
			iters = int(params[1])
		elif params[0] == '-k':
			k = int(params[1])

	data = np.genfromtxt(fileName + ".csv", delimiter = ',')
	res = k_means_alg(k, dtw, data, iters)

	serialize(data, res, prec, iters, k)
	print "Serialize done"
	
	pass

if __name__ == '__main__':
    main(sys.argv[1:])


