﻿using NQueen.Shared;
using NQueen.Shared.Enums;
using System.Collections.Generic;
using System.Linq;

namespace NQueen.Kernel
{
    public class SolverUnique : SolverBase
    {
        public SolverUnique(sbyte boardSize) : base(boardSize) { }

        public override IEnumerable<Solution> MainSolve()
        {
            if (DisplayMode == DisplayMode.Hide)
            { RecSolveConsoleForUniqueSolutions(0); }

            else
            { RecSolve(0); }

            return Solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }

        private void RecSolveConsoleForUniqueSolutions(sbyte colNo)
        {
            // All Symmetrical solutions are found and saved. Quit the recursion.
            if (QueenList[0] == HalfSize)
            { return; }

            // Here a new solution is found.
            if (colNo == BoardSize)
            {
                UpdateSolutions(QueenList);
                return;
            }

            QueenList[colNo] = LocateQueen(colNo);
            if (QueenList[colNo] == -1)
            { return; }

            var nextCol = (sbyte)(colNo + 1);
            RecSolveConsoleForUniqueSolutions(nextCol);
            RecSolveConsoleForUniqueSolutions(colNo);
        }
    }
}
