using NQueen.Shared;
using NQueen.Shared.Enums;
using NQueen.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NQueen.Kernel
{
    public class Solver : ISolver, ISolutionBuildup
    {
        public Solver(sbyte boardSize) => Initialize(boardSize);

        #region ISolverInterface

        public bool CancelSolver { get; set; }

        public SolutionMode SolutionMode { get; set; }

        public double ProgressValue { get; set; }

        public Task<ISimulationResults> GetSimulationResultsAsync(sbyte boardSize, SolutionMode solutionMode, DisplayMode displayMode = DisplayMode.Hide)
        {
            Initialize(boardSize);
            SolutionMode = solutionMode;
            DisplayMode = displayMode;
            return Task.Factory.StartNew(() => GetResults());
        }

        #endregion ISolverInterface

        #region ILivePresentation

        public int DelayInMilliseconds { get; set; }

        public DisplayMode DisplayMode { get; set; }

        public event QueenPlacedHandler QueenPlaced;

        public event SolutionFoundHandler SolutionFound;

        public event ProgressValueChangedHandler ProgressValueChanged;

        #endregion ILivePresentation

        public ISimulationResults GetResults()
        {
            var stopwatch = Stopwatch.StartNew();
            var solutions = MainSolve().ToList();
            stopwatch.Stop();
            var timeInSec = (double)stopwatch.ElapsedMilliseconds / 1000;
            var elapsedTimeInSec = Math.Round(timeInSec, 1);

            return new SimulationResults(solutions)
            {
                BoardSize = BoardSize,
                Solutions = solutions,
                NoOfSolutions = (SolutionMode == SolutionMode.All) ? NoOfSolutions : Solutions.Count,
                ElapsedTimeInSec = elapsedTimeInSec
            };
        }

        #region PublicProperties

        public HashSet<sbyte[]> Solutions { get; set; }

        public sbyte BoardSize { get; set; }

        public string BoardSizeText { get; set; }

        public int NoOfSolutions { get; set; }

        public sbyte HalfSize { get; set; }

        public sbyte[] QueenList { get; set; }

        #endregion PublicProperties

        protected virtual void OnProgressChanged(object sender, ProgressValueChangedEventArgs e) => ProgressValueChanged?.Invoke(this, e);

        protected virtual void OnQueenPlaced(object sender, QueenPlacedEventArgs e) => QueenPlaced?.Invoke(this, e);

        protected virtual void OnSolutionFound(object sender, SolutionFoundEventArgs e) => SolutionFound?.Invoke(this, e);

        #region PrivateMethods

        private IEnumerable<Solution> MainSolve()
        {
            // Recursive call to start the simulation
            if (SolutionMode == SolutionMode.All && DisplayMode == DisplayMode.Hide)
            { RecSolveConsoleForAllSolutions(0); }

            else if (SolutionMode == SolutionMode.Unique && DisplayMode == DisplayMode.Hide)
            { RecSolveConsoleForUniqueSolutions(0); }

            else
            { RecSolve(0); }

            return Solutions
                    .Select((s, index) => new Solution(s, index + 1));
        }

        // Recursive Solve Algorithm Simplified for Console, and SolutionMode.All
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

        private void RecSolve(sbyte colNo)
        {
            if (CancelSolver)
            { return; }

            // For SolutionMode == SolutionMode.Unique: If half sized is reached, quit the recursion.
            if (QueenList[0] == HalfSize)
            { return; }

            if (SolutionMode == SolutionMode.Single && NoOfSolutions == 1)
            { return; }

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
            RecSolve(nextCol);
            RecSolve(colNo);
        }

        private void UpdateSolutions(IEnumerable<sbyte> queens)
        {
            var solution = queens.ToArray();

            // If SolutionMode.Single, then we are done.
            if (SolutionMode == SolutionMode.Single)
            {
                Solutions.Add(solution);
                NoOfSolutions++;
                return;
            }

            // For SolutionMode.All: Increase NoOfAllSolutions, and save this solution, if MaxNoOfSolutionsInOutput not reached.
            if (SolutionMode == SolutionMode.All)
            {
                if (NoOfSolutions < Utility.MaxNoOfSolutionsInOutput)
                { Solutions.Add(solution); }

                NoOfSolutions++;
                return;
            }

            // For SolutionMode.Unique: Add this solution to Solutions, in case of no overlaps between Solutions and symmetricalSolutions.
            var symmetricalSolutions = Utility.GetSymmetricalSolutions(solution).ToList();

            if (!Solutions.Overlaps(symmetricalSolutions))
            { Solutions.Add(solution); }
        }

        // Locate Queen
        private sbyte LocateQueen(sbyte colNo)
        {
            for (sbyte pos = (sbyte)(QueenList[colNo] + 1); pos < BoardSize; pos++)
            {
                var isValid = true;
                for (int j = 0; j < colNo; j++)
                {
                    int lhs = Math.Abs(pos - QueenList[j]);
                    int rhs = Math.Abs(colNo - j);
                    if (0 != lhs && lhs != rhs)
                    { continue; }

                    isValid = false;
                    break;
                }

                if (isValid)
                { return pos; }
            }

            return -1;
        }

        private void Initialize(sbyte boardSize)
        {
            BoardSize = boardSize;
            CancelSolver = false;

            HalfSize = (sbyte)(BoardSize % 2 == 0 ?
                BoardSize / 2 :
                BoardSize / 2 + 1);

            QueenList = Enumerable.Repeat((sbyte)-1, BoardSize).ToArray();
            Solutions = new HashSet<sbyte[]>(new SequenceEquality<sbyte>());
        }

        #endregion PrivateMethods
    }
}
