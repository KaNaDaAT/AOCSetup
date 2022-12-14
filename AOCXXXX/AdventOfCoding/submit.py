from aocd import submit
import sys
import os


year = "XXXX"
if len(sys.argv) == 4:
	submit(sys.argv[3], year=int(year), day=int(sys.argv[1]), part=sys.argv[2])
	sys.exit();
print("Wrong parameter amount!")