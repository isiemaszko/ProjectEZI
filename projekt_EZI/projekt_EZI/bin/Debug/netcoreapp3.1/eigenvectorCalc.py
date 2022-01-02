from numpy import linalg as LA
import numpy as np


# wczytanie macierzy sąsiedztwa 
my_data = np.genfromtxt('matrix.csv', delimiter=',')

# obliczenie wartości i wektorów własnych
w, v = LA.eig(my_data)

# znalezienie największej wartości własnej
max_value = np.where(w == w.max())

# wzięcie wektora przy największej wartości własnej
a = v[max_value[0][0]].real

# normalizacja wartości od 0 do n, gdzie n = liczba stron
a_norm = np.interp(a, (a.min(), a.max()), (0,my_data.shape[0]))
print(a_norm)
np.savetxt('result.txt', a)