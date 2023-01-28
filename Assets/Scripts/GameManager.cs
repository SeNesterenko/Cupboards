using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PathFinder _pathFinder;
    [SerializeField] private MoveController _moveController;
    [SerializeField] private FileReader _fileReader;
    [SerializeField] private GraphController _graphController;

    [SerializeField] private List<PlayableSquare> _playableSquares;
    private Dictionary<int, Node> _nodes;

    private LevelSettings _levelSettings;
    private Node _currentNode;
    private PlayableSquare _currentPlayableSquare;
    private List<Node> _availableNodes;

    private void Awake()
    {
        _levelSettings = _fileReader.GetLevelSettings();
        _graphController.Initialize(_levelSettings);
    }

    private void Start()
    {
        _nodes = _graphController.GetNodes();
        _playableSquares = _graphController.GetPlayableSquares();
        
        foreach (var playableSquare in _playableSquares)
        {
            playableSquare.OnSelected += ShowAvailableNodes;
        }

        foreach (var node in _nodes)
        {
            node.Value.OnSelected += ChangePlayableSquarePosition;
        }
    }

    private void ShowAvailableNodes(Node startNode, PlayableSquare playableSquare)
    {
        FullResetNodes();

        _currentPlayableSquare = playableSquare;
        _availableNodes = _pathFinder.StartSearch(startNode);
    }

    private void ChangePlayableSquarePosition(Node newPositionNode)
    {
        FullResetNodes();
        
        _currentNode = newPositionNode;
        var starNode = _currentPlayableSquare.GetCurrentNode();

        if (!_availableNodes.Contains(_currentNode))
        {
            _currentNode.ChangeColor(ColorProvider.BlockedNode);
            _currentPlayableSquare = null;
            return;
        }
        
        var path = _pathFinder.GetPath(newPositionNode, starNode);

        if (_currentPlayableSquare != null)
        {
            _moveController.PlayPath(_currentPlayableSquare, path);
        }
        
        _currentPlayableSquare = null;
    }

    private void FullResetNodes()
    {
        if (_currentNode != null)
        {
            _currentNode.Reset();
        }

        if (_availableNodes != null)
        {
            foreach (var availableNode in _availableNodes)
            {
                availableNode.Reset();
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var playableSquare in _playableSquares)
        {
            playableSquare.OnSelected -= ShowAvailableNodes;
        }

        foreach (var node in _nodes)
        {
            node.Value.OnSelected -= ChangePlayableSquarePosition;
        }
    }
}