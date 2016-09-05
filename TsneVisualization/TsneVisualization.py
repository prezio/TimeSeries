import numpy as np
from sklearn.manifold import TSNE

def main(argv):
	data = np.genfromtxt("input.csv", delimiter = ',')
	model = TSNE(n_components = 2, random_state = 0)
	np.set_printoptions(suppress=True)
	np.savetxt("output.csv", model.fit_transform(X), delimiter=",")
	pass

if __name__ == '__main__':
    main(sys.argv[1:])