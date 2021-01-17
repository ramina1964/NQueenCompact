using NQueen.Shared.Enums;

namespace NQueen.Shared.Interfaces
{
    public delegate void QueenPlacedHandler(object sender, QueenPlacedEventArgs e);
    public delegate void SolutionFoundHandler(object sender, SolutionFoundEventArgs e);
    public delegate void ProgressValueChangedHandler(object sender, ProgressValueChangedEventArgs e);

    public interface ISolutionBuildup
    {
        bool CancelSolver { get; set; }

        int DelayInMilliseconds { get; set; }

        DisplayMode DisplayMode { get; set; }

        double ProgressValue { get; set; }

        event QueenPlacedHandler QueenPlaced;

        event SolutionFoundHandler SolutionFound;

        event ProgressValueChangedHandler ProgressValueChanged;
    }
}
