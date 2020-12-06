import sys
import numpy as np;
from datetime import datetime
from sklearn.linear_model import LinearRegression
import pandas as pd
from sklearn.preprocessing import StandardScaler, PolynomialFeatures

data = list(sys.argv[2].split(";"))
volume = list(sys.argv[1].split(" "))

#expectedDate = datetime.strptime(sys.argv[3], '%d.%m.%Y %H:%M:%S')
expectedDate = pd.to_datetime(sys.argv[3])
#print(expectedDate)

ss = StandardScaler()
volume_transformed = ss.fit_transform(np.array(volume).reshape(-1,1))

df2 = pd.DataFrame({'time': pd.to_datetime(data), 'data': volume_transformed.flatten()})

lr = LinearRegression()
#print(df2.time.values[0])
lr.fit(df2.time.values.reshape(-1, 1), df2.data.values.reshape(-1, 1))

user_entry = pd.to_datetime(expectedDate) 

#volume_pred = lr.predict([[user_entry.value]])
volume_pred = lr.predict(np.array(expectedDate.value).reshape(-1, 1))

print(ss.inverse_transform(volume_pred)[0][0])

