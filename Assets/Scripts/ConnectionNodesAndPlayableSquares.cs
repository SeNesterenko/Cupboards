public class ConnectionNodesAndPlayableSquares
{
    public Node Node { get; }
    public PlayableSquare PlayableSquare { get; }

    public ConnectionNodesAndPlayableSquares(Node node, PlayableSquare playableSquare)
    {
        Node = node;
        PlayableSquare = playableSquare;
    }
}