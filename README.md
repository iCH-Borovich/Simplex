# Simplex
Simplex alghorithm written in C# for one of my subjects in uni

How to use it?
Launch an a Simplex.exe in Debug, console application will pop up.
It will ask you to input input .txt file with boring math data, write a name/path or simply drag and drop and after spacebar write some random name for output file
Press enter and boom, magic, you got your answer...

Structure of the input file: 


𝑐1 𝑐2 𝑐3 ... 𝑐𝑛

//empty line 

𝑎11 𝑎12 𝑎13 ... 𝑎1𝑛 

𝑎21 𝑎22 𝑎23 ... 𝑎2𝑛 

... 

𝑎𝑚1 𝑎𝑚2 𝑎𝑚3 ... 𝑎𝑚𝑛 

//empty line 

𝑏1 𝑏2 𝑏3 ... 𝑏𝑚
 
*** Values should be tab-separated; lines should be separated by a system-specific 
line separator. 
____________________________

Structure of the output file:

a.  If the input file structure is incorrect (e.g., dimensions do not match), the 
output file should contain one line: 
 
ERROR: incorrect input data 
b.  If the specified problem is not stated in the standard form (e.g., has 
negative 𝑏 values), the output file should contain one line: 
 
ERROR: incorrect problem statement 
c.  If the input data is correct, the output file should contain a sequence of 
Simplex tableaus, separated by the empty line. Rows and columns of 
each Simplex tableau should be named (use x1, x2, ..., xn for original 
input variable and s1, s2, ..., sm for slack variable names). Values should 
be tab-separated; lines should be separated by a system-specific line 
separator. 
 
d.  If the LP problem is unbounded, the last line of the output file should 
contain: 
NO SOLUTION: unbounded problem 
e.  If the LP problem has a unique solution, the last lines of the output file 
should be: 
 
SOLUTION FOUND: unique solution 
Objective: 𝑧 (optimal value) 
𝑥1 𝑥2 𝑥3 ... 𝑥𝑛 𝑠1 𝑠2 𝑠3 ... 𝑠𝑚 
 
where the last line contains the optimal basic solution. 
 
f. If the LP problem has multiple solutions, the last lines of the output file 
should be: 
 
SOLUTION FOUND: multiple solutions 
Objective: 𝑧 (optimal value) 
𝑥1 𝑥2 𝑥3 ... 𝑥𝑛 𝑠1 𝑠2 𝑠3 ... 𝑠𝑚 
 
where the last line contains any of the optimal basic solutions.
____________________________
Decimal calculations are allowed; output values will be rounded to 3 decimal symbols
