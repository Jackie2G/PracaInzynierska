import sys
import numpy as np;
from datetime import datetime
from sklearn.linear_model import LinearRegression
import pandas as pd
from sklearn.preprocessing import StandardScaler, PolynomialFeatures

data = list(sys.argv[2].split(";"))
volume = list(sys.argv[1].split(" "))
print(data[0])
print(volume[0])
expectedDate = datetime.strptime(sys.argv[3], '%d.%m.%Y %H:%M:%S')

ss = StandardScaler()
volume_transformed = ss.fit_transform(np.array(volume).reshape(-1,1))
# y_transformed = np.array(y)

df2 = pd.DataFrame({'time': pd.to_datetime(data), 'data': volume_transformed.flatten()})

lr = LinearRegression()
print(type(df2.data.values[0]))
lr.fit(df2.time.values.reshape(-1, 1, df2.data.values.reshape(-1, 1)))

user_entry = pd.to_datetime(expectedDate) 

volume_pred = lr.predict([user_entry.value])

print(ss.reverse_transform(volume_pred))

