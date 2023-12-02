# AOCSetup

Welcome to AOCSetup - Your Advent of Code Project Setup!

## Overview

AOCSetup is designed to streamline the process of setting up your Advent of Code (AoC) project in C#. 
This tool automatically generates a C# project with all the necessary libraries, dependencies, and configurations for the chosen year. With AOCSetup, you can quickly dive into solving AoC challenges without worrying about the project setup.

## Features

- **Automatic Project Generation:** AOCSetup creates a C# project with the required structure for the selected Advent of Code year.
- **Library:** The setup includes a small library (under develop) that provides often-required helper functions, simplifying the coding process.
- **Configuration Setup:** AOCSetup handles the setup of configurations, making sure your project is ready to run without manual intervention.
- **User Interface:** The generated project includes a UI containing a console, giving you flexibility in how you want to interact with AoC challenges.

## Requirements

- **Python:** Make sure you have Python installed on your system.
- **Git:** Ensure that Git is installed.

> [!NOTE]
> This was created for Windows Systems. It may work on other systems with some effort.

## Setup

AOCSetup simplifies the setup process for your Advent of Code (AoC) project. 
Setting up your project can be achieved effortlessly by cloning this repository and running the setup.py script.

It will create the C# Project, fetch the data already available and setup a minimal git project.

1. **Clone Repository**
2. **Install Python Packages:**
   Run the following command to install the required Python packages: 
   `python pip-packages.py`
3. **Set AOC_SESSION Token:**
   Set the `AOC_SESSION` token as described on [Advent of Code Data](https://github.com/wimglenn/advent-of-code-data). 
   You can set it directly in system variables on Windows.
4. **Run Setup Script:**
   Run the `setup.py` script in administrator mode to allow necessary configurations:
   `python setup.py`
5. **Open .sln File with Visual Studio:**
   Open the generated `.sln` file with Visual Studio to start coding your Advent of Code solutions.

## User Inteface

The user interface for AOCSetup provides a convenient way to interact with your Advent of Code (AoC) solutions. Whether you prefer button clicks or command-line interactions, the interface supports both methods. The buttons in the user interface are designed to execute various commands, streamlining the process of running, debugging, submitting, and managing your AoC solutions.

## Commands

You can use command-line interactions. The available commands include:

- `start Day01A`: Runs the code for Day 1 Part 1.
- `start Day01A -d true`: Runs Day 1 Part 1 with debug output (debug output can be set in the script).
- `submit Day01A`: Submits the result of Day 1 Part 1.
- `data Day01A`: Opens the data file for Day 1 Part 1, allowing you to use alternative debug inputs.
- `fetch Day01`: Fetches the data for Day 1 if needed.

> [!NOTE]
> 
> `Day` can be ommitet

#### Buttons

- Clicking a button will execute the corresponding `start` command.
- Holding **SHIFT** while clicking will run the command with **debug** output.
- Holding **ALT** while clicking will **submit** the result.
- Holding **CTRL** while clicking will **open** the **data** file.
> [!NOTE]
> Each button click will check if data needs to be fetched before executing the command.

### Contributions

Contributions to AOCSetup are welcome! If you have ideas for improvements or want to add support for additional features, feel free to open issues or pull requests.

## Happy Coding!

Spend less time on setup and more time on solving Advent of Code challenges. Enjoy your coding journey with AOCSetup!


