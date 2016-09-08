import numpy as np
from sklearn.manifold import TSNE
import matplotlib.pyplot as plt
import matplotlib.cm as cm
import sys

def main(argv):
	data = np.genfromtxt("input.csv", delimiter = ',')
	model = TSNE(n_components = 2, random_state = 0)
	np.set_printoptions(suppress=True)

	data = model.fit_transform(data)

	#cmap = plt.get_cmap('gnuplot')

	x = data[:, 0]
	y = data[:, 1]
	colors = cm.rainbow(np.linspace(0, 1, len(y)))

	plt.scatter(x, y, color=colors)

	plt.savefig("output.png")
	plt.show()
	plt.clf()
	np.savetxt("output.csv", data, delimiter=",")
	pass

if __name__ == '__main__':
    main(sys.argv[1:])