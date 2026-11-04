"""Example integration: run while the dashboard is in REAL mode and STARTED."""
import requests
URL='http://127.0.0.1:5000/api/detection'
# Replace this example with the classification result returned by your own AI model.
result={'category':'NG_RI_SET','confidence':0.967,'source':'ai'}
response=requests.post(URL,json=result,timeout=5)
print(response.status_code,response.json())
