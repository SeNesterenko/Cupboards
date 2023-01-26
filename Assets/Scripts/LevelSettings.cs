using System.Collections.Generic;
using UnityEngine;

public class LevelSettings : MonoBehaviour
{
    public int QuantityPlayableSquares { get; set; }
    public int QuantityNodes { get; set; }
    public List<CustomVector2> NodePositions { get; set; }
    public List<int> StartPositions { get; set; }
    public List<int> FinishPositions { get; set; }
    public int QuantityConnections { get; set; }
    public List<CustomVector2> Connections { get; set; }
}

public class CustomVector2
{
    public int X { get; set; }
    public int Y { get; set; }
}