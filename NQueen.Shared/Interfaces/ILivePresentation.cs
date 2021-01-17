using NQueen.Shared.Enums;

namespace NQueen.Shared.Interfaces
{
    public interface ILivePresentation
    {
        int DelayInMilliseconds { get; set; }

        DisplayMode DisplayMode { get; set; }

        double ProgressValue { get; set; }

        event QueenPlacedHandler QueenPlaced;

        event SolutionFoundHandler SolutionFound;

        event ProgressValueChangedHandler ProgressValueChanged;
    }
}
