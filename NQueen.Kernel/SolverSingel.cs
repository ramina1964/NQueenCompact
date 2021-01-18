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

        private bool RecSolveConsoleForSingelSolutions(sbyte colNo)
        {
            if (colNo == -1)
            { return false; }

            // Because of a symmetrical board, there is no need to continue.
            if (QueenList[0] == HalfSize)
            { return false; }

            if (Solutions.Count == 1)
            { return false; }

            // The solution is found.
            if (colNo == BoardSize)
            {
                Solutions.Add(QueenList.ToArray());
                return true;
            }

            QueenList[colNo] = LocateQueen(colNo);
            if (QueenList[colNo] == -1)
            { return false; }

            var nextCol = (sbyte)(colNo + 1);
            return RecSolveConsoleForSingelSolutions(nextCol) || RecSolveConsoleForSingelSolutions(colNo);
        }
    }
}