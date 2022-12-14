import os
import shutil

file_path = os.path.dirname(__file__)
suffix_d = ".data"
suffix_cs = ".cs"
for i in range(1, 26):
    current_num = str(i).zfill(2)
    current_file_path = os.path.join(file_path, "Day" + current_num)
    # Create Folder
    if not os.path.exists(current_file_path):
        os.mkdir(current_file_path)
    day_A = "Day" + current_num + "A"
    day_B = "Day" + current_num + "B"

    # Create cs files
    cs_file_A = os.path.join(current_file_path, day_A + suffix_cs)
    cs_file_B = os.path.join(current_file_path, day_B + suffix_cs)
    content = ""
    with open(os.path.join(file_path, "DayPref.cs"), 'r') as fp:
        content = fp.read()

    if not os.path.exists(cs_file_A):
        with open(cs_file_A, 'w') as fp:
            acontent = content.replace("DayPref", day_A)
            fp.write(acontent)
    if not os.path.exists(cs_file_B):
        with open(cs_file_B, 'w') as fp:
            acontent = content.replace("DayPref", day_B)
            fp.write(acontent)
