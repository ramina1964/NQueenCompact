﻿namespace NQueen.Shared.Interfaces
{
    public interface ISolution
    {
        string Details { get; }

        int? Id { get; }

        string Name { get; }

        sbyte[] QueenList { get; }

        string ToString();
    }
}