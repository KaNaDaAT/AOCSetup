from aocd import get_data
import sys
import os
import datetime
import time

data = "empty"
year = "XXXX"
suffix_d = ".data"
file_path = os.path.dirname(__file__)
file_path = os.path.join(file_path, "Days", "Day")
path = ""
if len(sys.argv) == 2:
    current_num = str(sys.argv[1]).zfill(2)
    current_file_path = file_path + current_num
    day_A = "Day" + current_num + "A"
    day_B = "Day" + current_num + "B"
    data_file_A = os.path.join(current_file_path, day_A + suffix_d)
    data_file_B = os.path.join(current_file_path, day_B + suffix_d)

    try:
        data = get_data(day=int(sys.argv[1]), year=int(year))

        if not os.path.exists(data_file_A):
            os.makedirs(os.path.dirname(data_file_A), exist_ok=True)
            with open(data_file_A, 'w') as fp:
                fp.write(data)
        if not os.path.exists(data_file_B):
            os.makedirs(os.path.dirname(data_file_B), exist_ok=True)
            with open(data_file_B, 'w') as fp:
                fp.write(data)
    except:
        pass
else:
    for i in range(1, 26):
        current_num = str(i).zfill(2)
        current_file_path = file_path + current_num
        day_A = "Day" + current_num + "A"
        day_B = "Day" + current_num + "B"
        data_file_A = os.path.join(current_file_path, day_A + suffix_d)
        data_file_B = os.path.join(current_file_path, day_B + suffix_d)

        try:
            data = get_data(day=i, year=int(year))
            os.makedirs(os.path.dirname(data_file_A), exist_ok=True)
            with open(data_file_A, 'w') as fp:
                fp.write(data)
            os.makedirs(os.path.dirname(data_file_B), exist_ok=True)
            with open(data_file_B, 'w') as fp:
                fp.write(data)
        except Exception as e:
            if "not available yet" in str(e):
                sys.exit()
            print(e)
    time.sleep(100)
