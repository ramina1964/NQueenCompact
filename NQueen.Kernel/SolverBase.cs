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
    public abstract class SolverBase : ISolver, ISolutionBuildup
    {
        public SolverBase(sbyte boardSize) => Initialize(boardSize);

        #region ISolver

        public SolutionMode SolutionMode { get; set; }

        public Task<ISimulationResults> GetSimulationResultsAsync(
            sbyte boardSize, SolutionMode solutionMode,
            DisplayMode displayMode = DisplayMode.Hide)
        {
            Initialize(boardSize);
            SolutionMode = solutionMode;
            DisplayMode = displayMode;
            return Task.Factory.StartNew(() => GetResults());
        }

        #endregion ISolver

        #region ISolutionBuilup

        public bool CancelSolver { get; set; }

        public int DelayInMilliseconds { get; set; }

        public DisplayMode DisplayMode { get; set; }

        public double ProgressValue { get; set; }

        public event QueenPlacedHandler QueenPlaced;

        public event SolutionFoundHandler SolutionFound;

        public event ProgressValueChangedHandler ProgressValueChanged;

        #endregion ISolutionBuildup

        #region PublicAttributes

        public HashSet<sbyte[]> Solutions { get; set; }

        public sbyte BoardSize { get; set; }

        public string BoardSizeText { get; set; }

        public int NoOfSolutionsAll { get; set; }

        public sbyte HalfSize { get; set; }

        public sbyte[] QueenList { get; set; }

        public static SolverBase GetSolver(sbyte boardSize, SolutionMode solutionMode) =>
            solutionMode switch
            {
                SolutionMode.Single => new SolverSingel(boardSize),
                SolutionMode.Unique => new SolverUnique(boardSize),
                SolutionMode.All => new SolverAll(boardSize),
                _ => throw new NotImplementedException(),
            };

        #endregion PublicAttributes

        public ISimulationResults GetResults()
        {
            var stopwatch = Stopwatch.StartNew();
            var solutions = MainSolver().ToList();
            stopwatch.Stop();
            var timeInSec = (double)stopwatch.ElapsedMilliseconds / 1000;
            var elapsedTimeInSec = Math.Round(timeInSec, 1);

            return new SimulationResults(solutions)
            {
                BoardSize = BoardSize,
                Solutions = solutions,
                TotalNoOfSolutions = (SolutionMode == SolutionMode.All) ? NoOfSolutionsAll : Solutions.Count,
                ElapsedTimeInSec = elapsedTimeInSec
            };
        }

        protected virtual void OnProgressChanged(object sender, ProgressValueChangedEventArgs e) => ProgressValueChanged?.Invoke(this, e);

        protected virtual void OnQueenPlaced(object sender, QueenPlacedEventArgs e) => QueenPlaced?.Invoke(this, e);

        protected virtual void OnSolutionFound(object sender, SolutionFoundEventArgs e) => SolutionFound?.Invoke(this, e);

        protected abstract IEnumerable<Solution> MainSolver();

        protected bool RecSolve(sbyte colNo)
        {
            if (CancelSolver)
            { return false; }

            // For SolutionMode == SolutionMode.Unique: If half sized is reached, quit the recursion.
            if (QueenList[0] == HalfSize)
            { return false; }

            if (SolutionMode == SolutionMode.Single && Solutions.Count == 1)
            { return true; }

            if (colNo == -1)
            { return false; }

            // A new solution is found.
            if (colNo == BoardSize)
            {
                UpdateSolutions(QueenList);
                return true;
            }

            QueenList[colNo] = PlaceQueen(colNo);
            if (QueenList[colNo] == -1)
            { return false; }

            var nextCol = (sbyte)(colNo + 1);
            return RecSolve(nextCol) || RecSolve(colNo);
        }

        protected void UpdateSolutions(IEnumerable<sbyte> queenList)
        {
            // Must cache queenLIst because the underlying argument is continuously changing under the simulation.
            var solution = queenList.ToArray();

            // If SolutionMode.Single, then we are done.
            if (SolutionMode == SolutionMode.Single)
            {
                Solutions.Add(solution);
                return;
            }

            // For SolutionMode.All: Increase NoOfAllSolutions, and save this solution, if MaxNoOfSolutionsInOutput not reached.
            if (SolutionMode == SolutionMode.All)
            {
                if (NoOfSolutionsAll < Utility.MaxNoOfSolutionsInOutput)
                { Solutions.Add(solution); }

                NoOfSolutionsAll++;
                return;
            }

            // For SolutionMode.Unique: Add this solution to Solutions, in case of no overlaps between Solutions and symmetricalSolutions.
            var symmetricalSolutions = Utility.GetSymmetricalSolutions(solution).ToList();

            if (!Solutions.Overlaps(symmetricalSolutions))
            { Solutions.Add(solution); }
        }

        // Place Queen
        protected sbyte PlaceQueen(sbyte colNo)
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

        protected void Initialize(sbyte boardSize)
        {
            BoardSize = boardSize;
            CancelSolver = false;

            HalfSize = (sbyte)(BoardSize % 2 == 0 ?
                BoardSize / 2 :
                BoardSize / 2 + 1);

            QueenList = Enumerable.Repeat((sbyte)-1, BoardSize).ToArray();
            Solutions = new HashSet<sbyte[]>(new SequenceEquality<sbyte>());
        }

    }
}
