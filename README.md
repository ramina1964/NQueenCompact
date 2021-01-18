# NQueenCompact
This is a Console Application developed in .Net 5.0 with Visual Studio 2019 for solving the classical N-Queen Problem:
Given a general N x N chess board, place N queens on it without any of them hits the other ones.

There are different approaches for solving this problem. We have used Backtracking. Here is a description of the algorithm for finding a single solution:
  - Begin from the lower left corner, and go columnwise first, then row by row.
  - Place a queen in the first allowed column, and row.
  - If successful continue to the next column, find the first vacant position, and do the same.
  - If it is not possible to place the queen, we must go back a column, remove the queen and place it upward.
  - Follow the procedure above until the queen is placed, then move forward to the next column.  
  - Follow this procedure until all N queens are placed, or possibly no solution is found.
  
When a solution is found we may continue the procedure to find several solutions. This is done by removing the last queen and trying to place it upward, as above.
Solution packs may be categorized in three different modes, as follows:
  - Single Solution: Consists of a single solution, the first one found.
  - Unique Solutions: None of the solutions in this mode may be symmetrical counter part of the others, see below.
  - All Solutions: The unique solutions as well as their symmetrical counter parts.
Actually the no. of problem's solutions grows exponentially, which is a challege for the available resources, both RAM and the processor.
    
A solution may have up to 7 symmetrical counterparts. These are found as follws:
  - The solution's mirror image with respect to the Horizontal mid-line.
  - The solution's mirror image with respect to the vertical mid-line.
  - The solution's mirror image with respect to the main diagonal from lower left to upper right.
  - The solution's mirror image with respect to the bi-diagonal from lower right to upper left.
  - By rotating the solution 90 degrees counter clockwise.
  - By rotating the solution 180 degrees counter clockwise
  - By rotating the solution 270 degrees counter clockwise.
  
So the solutions appear in packages of up to 8. Note that for small board sizes many of these solutions are coincidental.
Note also that when N increases the relation "all solutions / unique solutions" approches the value eight.
Because of excessive memory usage, specially with "All Solutions", we only save a handful of them, but find the solutions count, as well as the elapsed time.
