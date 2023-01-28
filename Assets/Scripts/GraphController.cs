using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    [SerializeField] private Node _prefabNode;
    [SerializeField] private PlayableSquare _prefabPlayableSquare;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private LineRenderer _prefabLine;
    
    
    private LevelSettings _levelSettings;
    private Dictionary<int, Node> _nodes;
    private List<PlayableSquare> _playableSquares;

    public void Initialize(LevelSettings levelSettings)
    {
        _levelSettings = levelSettings;
        
        CrateNodes();
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

    private void DrawLine(Node startNode, Node endNode)
    {
        var line = Instantiate(_prefabLine, _canvas.transform);
        line.material.color = ColorProvider.LineColor;
        var lineIndex = 0;
        
        line.SetPosition(lineIndex, startNode.Position.anchoredPosition);
        lineIndex++;

        line.SetPosition(lineIndex, endNode.Position.anchoredPosition);
        lineIndex++;
    }
    
    private void CreatePlayableSquares()
    {
        _playableSquares = new List<PlayableSquare>();
        
        for (var i = 0; i < _levelSettings.QuantityPlayableSquares; i++)
        {
            var playableSquare = Instantiate(_prefabPlayableSquare, _canvas.transform);
            playableSquare.SetPosition(_nodes[_levelSettings.StartPositions[i]]);

            var color = GenerateColor();
            playableSquare.SetColor(color);
            _playableSquares.Add(playableSquare);
        }
    }

    private void CrateNodes()
    {
        _nodes = new Dictionary<int, Node>();

        for (var i = 0; i < _levelSettings.QuantityNodes; i++)
        {
            var node = Instantiate(_prefabNode, _canvas.transform);
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