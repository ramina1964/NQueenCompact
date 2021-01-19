using NQueen.Shared.Enums;
using System.Threading.Tasks;

namespace NQueen.Shared.Interfaces
{
    public interface ISolver
    {
        SolutionMode SolutionMode { get; set; }

        Task<ISimulationResults> GetSimulationResultsAsync(
            sbyte boardSize, SolutionMode solutionMode,
            DisplayMode displayMode = DisplayMode.Hide);
    }
}