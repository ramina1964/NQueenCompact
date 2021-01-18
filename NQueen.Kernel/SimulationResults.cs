using NQueen.Shared;
using NQueen.Shared.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NQueen.Kernel
{
    public class SimulationResults : ISimulationResults
    {
        public SimulationResults(IEnumerable<Solution> allSolutions)
        {
            Debug.Assert(allSolutions != null, "allSolutions != null");
            var enumerable = allSolutions as IList<Solution> ?? allSolutions.ToList();
            var sol = enumerable.FirstOrDefault();
            if (sol == null)
            {
                TotalNoOfSolutions = 0;
                Solutions = new List<Solution>();
            }
            else
            {
                BoardSize = (sbyte)sol.Positions.Count;
                TotalNoOfSolutions = enumerable.Count;
                Solutions = new List<Solution>(enumerable);
            }
        }

        #region ISimulationResult

        public sbyte BoardSize { get; set; }

        public IEnumerable<Solution> Solutions { get; set; }

        public int TotalNoOfSolutions { get; set; }

        public double ElapsedTimeInSec { get; set; }

        #endregion ISimulationResult
    }
}