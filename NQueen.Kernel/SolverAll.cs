using NQueen.Shared;
using NQueen.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace NQueen.Kernel
{
    public class SolverAll : SolverBase
    {
        public SolverAll(sbyte boardSize) : base(boardSize) { }

        public override IEnumerable<Solution> MainSolve()
        {
            // Recursive call to start the simulation
            if (DisplayMode == DisplayMode.Hide)
            { RecSolveConsoleForAllSolutions(0); }

            else
            { RecSolve(0); }

            return Solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }

        private void RecSolveConsoleForAllSolutions(sbyte colNo)
        {
            if (colNo == -1)
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
            RecSolveConsoleForAllSolutions(nextCol);
            RecSolveConsoleForAllSolutions(colNo);
        }
    }
}
