using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    [SerializeField] private Node _prefabNode;
    [SerializeField] private PlayableSquare _prefabPlayableSquare;
    [SerializeField] private LineRenderer _prefabLine;
    [SerializeField] private PlayableSquare _prefabFinalPositionPlayableSquare;

    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private Canvas _finalPositionsCanvas;
    
    private Dictionary<int, Node> _nodes;
    private List<PlayableSquare> _playableSquares;
    private List<ConnectionNodesAndPlayableSquares> _finalPositions;
    private LevelSettings _levelSettings;

    public void Initialize(LevelSettings levelSettings)
    {
        _levelSettings = levelSettings;
        
        CreateNodes();
        CreatePlayableSquares();
    }

    public Dictionary<int, Node> GetNodes()
    {
        return _nodes;
    }

    public List<PlayableSquare> GetPlayableSquares()
    {
        return _playableSquares;
    }

    public List<ConnectionNodesAndPlayableSquares> GetFinalPositions()
    {
        return _finalPositions;
    }

    private void DrawLine(Node startNode, Node endNode)
    {
        var line = Instantiate(_prefabLine, _mainCanvas.transform);
        line.material.color = ColorProvider.LineColor;
        var lineIndex = 0;
        
        line.SetPosition(lineIndex, startNode.Position.anchoredPosition);
        lineIndex++;

        line.SetPosition(lineIndex, endNode.Position.anchoredPosition);
        lineIndex++;
    }
    
    private void CreatePlayableSquares()
    {
        _finalPositions = new List<ConnectionNodesAndPlayableSquares>();
        _playableSquares = new List<PlayableSquare>();
        
        for (var i = 0; i < _levelSettings.QuantityPlayableSquares; i++)
        {
            InstantiatePlayableSquares(i);
        }
    }

    private void InstantiatePlayableSquares(int i)
    {
        var playableSquare = Instantiate(_prefabPlayableSquare, _mainCanvas.transform);
        playableSquare.SetPosition(_nodes[_levelSettings.StartPositions[i]]);

        var color = GenerateColor();
        playableSquare.SetColor(color);
        _playableSquares.Add(playableSquare);

        var finalPositionsPlayableSquare = Instantiate(_prefabFinalPositionPlayableSquare, _finalPositionsCanvas.transform);
        finalPositionsPlayableSquare.SetPosition(_nodes[_levelSettings.FinishPositions[i]].Position.anchoredPosition / 2);

        finalPositionsPlayableSquare.SetColor(playableSquare.GetColor());

        _finalPositions.Add(
            new ConnectionNodesAndPlayableSquares(_nodes[_levelSettings.FinishPositions[i]], playableSquare));
    }

    private void CreateNodes()
    {
        _nodes = new Dictionary<int, Node>();

        for (var i = 0; i < _levelSettings.QuantityNodes; i++)
        {
            var node = Instantiate(_prefabNode, _mainCanvas.transform);
            node.SetPosition(_levelSettings.NodePositions[i].X, _levelSettings.NodePositions[i].Y);
            _nodes[i+1] = node;
        }

        foreach (var connection in _levelSettings.Connections)
        {
            DrawLine(_nodes[connection.X], _nodes[connection.Y]);
            _nodes[connection.X].AddNeighbour(_nodes[connection.Y]);
            _nodes[connection.Y].AddNeighbour(_nodes[connection.X]);
        }
    }

    private static Color GenerateColor()
    {
        var r = Random.Range(0, 255) / 255f;
        var g = Random.Range(0, 255) / 255f;
        var b = Random.Range(0, 255) / 255f;
        var color = new Color(r, g, b);
        
        return color;
    }
}