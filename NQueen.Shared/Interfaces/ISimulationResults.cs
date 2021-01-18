﻿using System.Collections.Generic;

namespace NQueen.Shared.Interfaces
{
    public interface ISimulationResults
    {
        sbyte BoardSize { get; }

        IEnumerable<Solution> Solutions { get; }

        int TotalNoOfSolutions { get; }

        double ElapsedTimeInSec { get; }
    }
}