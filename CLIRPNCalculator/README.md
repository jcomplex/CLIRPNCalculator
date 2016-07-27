# CLI RPN Calculator
A C# command-line Reverse Polish Notation (RPN) calculator for users who use UNIX-like CLI utilities.

## Installation
Unzip the attached zip folder and place folder in desginated directory.

## How To Run
There are two ways to run this program:
1. In the command-prompt window, navigate to the folder that contains your newly created directory and type the following command 
    ```
    > CLIRPNCalculator.exe
    ```
2. Double click on the CLIRPNCalculator.exe in the designated directory

## Basic Usage
#### How It Works
Starting the application should give some basic directions:

![Calculator ready.  Type '?' for help.](/screenshot.png?raw=true)

There are two ways to enter expressions into the calculator.

**Inline**
```
> 5 8 +
13
```

**Per Line**
```
> 5
5
> 8
8
> +
13
```

#### Operators
The following operators are allowed:
* addition (+)
* subtraction (-)
* division (/)
* multiplication (*)

#### Commands
The following are the accepted commands:
* q  - exits the program
* c  - clears the last entry from memory
* ce - resets the calculator
* ?  - displays help message
* v  - the version of the software
* h  - history of the current equation (view of the stack)

## Built With
* Visual Studio 2015

## Authors
**Joya Joseph**
