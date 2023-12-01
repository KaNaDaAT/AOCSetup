import os
import shutil
import sys
import tkinter as tk
from tkinter import simpledialog
import glob
import subprocess
import datetime

file_path = os.path.dirname(__file__)
AOC_path = os.path.join(file_path, "AOCXXXX")
year = "XXXX"

ROOT = tk.Tk()
ROOT.withdraw()
if len(sys.argv) < 2:
    while not year.isnumeric():
        year = simpledialog.askstring(title="Year", prompt="Which year?", initialvalue=datetime.date.today().year)
else:
    year = str(int(sys.argv[1]))

new_AOC_path = os.path.join(file_path, "AOC" + year)

if os.path.exists(new_AOC_path):
    paths = glob.glob(new_AOC_path + "**/**", recursive=False)
    if len(paths) != 0:
        print("Already exists")
        sys.exit()

shutil.copytree(AOC_path, new_AOC_path, dirs_exist_ok=True)

for filename in glob.iglob(new_AOC_path + "**/**", recursive=True):
    content = ""
    try:
        if os.path.isfile(filename):
            with open(filename, 'r', encoding="utf8") as fp:
                content = fp.read()
            content = content.replace("XXXX", year)
            content = content.replace("YEAR = 0000", "YEAR = " + year)
            with open(filename, 'w', encoding="utf8") as fp:
                fp.write(content)
    except:
        pass
    if "XXXX" in filename:
        shutil.move(filename, filename.replace("XXXX", year))

createDays_path = os.path.join(new_AOC_path, "AdventOfCoding", "Days",
                               "createDays.py")
subprocess.run(["python", createDays_path])
createData_path = os.path.join(new_AOC_path, "AdventOfCoding", "data.py")
subprocess.run(["python", createData_path])

os.chdir(new_AOC_path)
git_commands = [
    "git init", "git add .", f"git commit -m \"AOC {year} Initial commit\""
]
for command in git_commands:
    subprocess.run(command, shell=True)
