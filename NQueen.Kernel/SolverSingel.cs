using NQueen.Shared;
using NQueen.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace NQueen.Kernel
{
    public class SolverSingel : SolverBase
    {
        public SolverSingel(sbyte boardSize) : base(boardSize) { }

        public override IEnumerable<Solution> MainSolve()
        {
            if (DisplayMode == DisplayMode.Hide)
            { RecSolveConsoleForSingelSolutions(0); }

            else
            { RecSolve(0); }

            return Solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }

        private void RecSolveConsoleForSingelSolutions(sbyte colNo)
        {
            if (colNo == -1)
            { return; }

            // For SolutionMode == SolutionMode.Unique: If half sized is reached, quit the recursion.
            if (QueenList[0] == HalfSize)
            { return; }

            if (NoOfSolutions == 1)
            { return; }

            // A new solution is found.
            if (colNo == BoardSize)
            {
                UpdateSolutions(QueenList);
                return;
            }

            QueenList[colNo] = LocateQueen(colNo);
            if (QueenList[colNo] == -1)
            { return; }

            var nextCol = (sbyte)(colNo + 1);
            RecSolveConsoleForSingelSolutions(nextCol);
            RecSolveConsoleForSingelSolutions(colNo);
        }
    }
}