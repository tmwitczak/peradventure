import itertools

import matplotlib.pyplot as plt
import numpy as np
from sklearn.cluster import KMeans
from sklearn.preprocessing import MinMaxScaler


level_duration = 30

hands_at_once = np.arange(1, 3 + 1)
birds_at_once = np.arange(1, 2 + 1)
smoke_at_once = np.arange(1, 4 + 1)

# hand_interval

# hand_waves = level_duration / hand_interval

# hand_volume = hands_at_once * hand_waves
#             = hands_at_once * (level_duration / hand_interval)

hand_volume_range = np.concatenate((np.array([0]), np.arange(10, 30 + 1)))
bird_volume_range = np.concatenate((np.array([0]), np.arange(5, 30 + 1)))
smoke_volume_range = np.concatenate((np.array([0]), np.arange(1, 12 + 1)))


def difficulty(volumes, factors=np.array([1, 1, 1])):
    return np.dot(volumes, factors)


volumes = []
for hand_volume in hand_volume_range:
    for bird_volume in bird_volume_range:
        for smoke_volume in smoke_volume_range:
            volumes.append([hand_volume, bird_volume, smoke_volume])
volumes = np.asarray(volumes)

difficulties = difficulty(volumes)
difficulties = (difficulties - np.min(difficulties)) / (
    np.max(difficulties) - np.min(difficulties)
)

indices = difficulties.argsort()
volumes = volumes[indices]
difficulties = difficulties[indices]

i = int(volumes.shape[0] * 0.005)
v = volumes[i]
d = difficulties[i]
print(i)
print(v)
print(d)
ho = np.random.choice(hands_at_once[1:], 1)[0]
hi = (ho * level_duration) / v[0]
print(ho, "hands at once *", hi, "interval")

kmeans = KMeans()
scaler = MinMaxScaler()
labels = kmeans.fit_predict(scaler.fit_transform(volumes))

# plt.hist(difficulties, bins=10)
fig = plt.figure()
fig.add_subplot((111), projection="3d").scatter(
    volumes[:, 0], volumes[:, 1], volumes[:, 2], difficulties, c=labels
)
plt.show()

