using NQueen.Shared.Enums;

namespace NQueen.Shared.Interfaces
{
    public interface ISolver
    {
        SolutionMode SolutionMode { get; set; }

        ISimulationResults GetResults();
    }
}