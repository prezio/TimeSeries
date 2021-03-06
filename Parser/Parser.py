# Skrypt wyciaga dane na temat notowan spolki i zapisuje przedzialami do wyj?ciowego csv.
# Nadmiarowe przedzialy sa usuwane

import csv
import time
from datetime import datetime
import numpy as np
import sys

def parse_from_csv(reader, win_length, win_shift, radius, prec):
	d = {}
	for row in reader:
		t1 = row[6]
		d1 = row[5]
		v1 = float(row[3])
		a1 = int(row[4])

		tmp = d1 + t1
		if len(d1) == 7:
			tmp = '0' + tmp

		key = int(time.mktime(datetime.strptime(tmp, '%d%m%Y%H:%M:%S').timetuple()))
		if key in d.keys():
			old = d[key]
			d[key] = (old[0] + v1 * a1, old[1] + a1)
		else:
			d[key] = (v1 * a1, a1)
		print (key)
	
	
	serie = np.array(map(lambda x: x[0] / x[1], d.values()))
	smooth = np.zeros(serie.shape)
	size = len(serie)

	for j in range(size):
		amount = 0
		for k in range(max(j-radius, 0), min(j+radius, size)):
			smooth[j] += serie[k]
			amount += 1
		smooth[j] /= amount

	serie = smooth
	ret = []
	i = 0

	while i + win_length <= size:
		tmp = serie[i:i+win_length]
		if (i != 0):
			#sprawdzamy czy warto dodac dany przedzial
			min_val = min(tmp[-win_shift:])
			max_val = max(tmp[-win_shift:])
			last_val = last[-1]
			if ( abs(max_val - min_val) >= prec or abs(max_val - last_val) >= prec or abs(min_val - last_val) >= prec):
				ret.append(serie[i:i + win_length])
		else:
			ret.append(serie[i:i + win_length])
		last = ret[-1]
		i+=win_shift

	return np.array(ret)

def main(argv):
	csvfile = open('t_TradeReport.csv', 'rb')
	reader = csv.reader(csvfile)

	data = parse_from_csv(reader, 100, 5, 20, -1)
	
	np.savetxt("output.csv", data, delimiter=",")
	pass

if __name__ == '__main__':
	main(sys.argv[1:])