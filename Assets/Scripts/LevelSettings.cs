using System;
using System.Collections.Generic;

[Serializable]
public class LevelSettings
{
    public int QuantityPlayableSquares;
    public int QuantityNodes;
    public List<CustomVector2> NodePositions;
    public List<int> StartPositions;
    public List<int> FinishPositions;
    public int QuantityConnections;
    public List<CustomVector2> Connections;
}

[Serializable]
public class CustomVector2
{
    public int X;
    public int Y;
}