ET2Solver - Eternity 2 Puzzle Solver
Author: Joe Cotroneo
Created: 2009-2010

License: Free for personal use
Warranties: None

Status
------
Experimental, some things work, some may have bugs.

Description
-----------
This is a hodge podge of ideas I had when exploring the Eternity 2 Puzzle back in 2009-2010.

Features
--------
Backtracking Solver with logging/stats tracking (not optimized for performance, places 130k tiles/sec on 16x16 board using single gen9 i7 cpu core)
Note: Solver2 (2x2) is incomplete
Interactive mode editor/solver
Board generator - generate E2 like boards of various sizes
Board designer - create your own custom board designs

Notes
-----
Tile notation - I found it easier to use ascii characters A-V to identify each tile pattern, rather than the numeric form used in other solvers
- : edge pattern
A..E : border patterns (24 pattern halves each)
F..J : inner patterns group 1 (48 patterns halves each)
K..V : inner patterns group 2 (50 patterns halves each)

Building From Source
--------------------
Language: C#
Framework: .NET 4.8 (originally developed using SharpDevelop)
# dotnet build

Instructions
------------
open bin/ET2Solver-debug.exe

Manual / Interactive Solver
---------------------------
1. select Load/Save tab
2. load a tileset
3. select Board tab
4. click the board cell where you wish to place a tile
5. click the Search button to find matching available tiles
6. click any of the results in the sidebar to place the tile

Remove a tile from the board: middle click
Rotate a tile: right click

Run Auto Solver on 10x10 using valid border as hint
---------------------------------------------------
The border is copied as-is from the solved board to test the theory of being able to solve the puzzle when using the correct border as a hint.
A solution will be found within approx 34.6m iterations
You can pause the backtracker and view the board to see current progress, then resume

1. select Load/Save tab
2. select board layout 10x10
3. click Set layout
4. load 10x10x4x10_61 tileset
5. click Load Model, open file 'border'
6. click Board tab, confirm that border has been loaded
7. click Set Model as Hint
8. select Solver1 tab
9. enable randomize dataset
10. select Solve Method: 10x10_border_rows
11. click Start

Auto pause when solution found
------------------------------
1. enable score trigger
2. enter score trigger (eg. enter 100 for 10x10, or 256 for 16x16)
3. select pause when triggered
4. start solver
