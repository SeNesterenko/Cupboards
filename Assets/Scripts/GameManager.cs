using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveController))]
[RequireComponent(typeof(GraphController))]
[RequireComponent(typeof(WinController))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private GameObject _mainCanvas;
    
    [SerializeField] private FileReader _fileReader;
    [SerializeField] private PathFinder _pathFinder;
    
    [SerializeField] private MoveController _moveController;
    [SerializeField] private GraphController _graphController;
    [SerializeField] private WinController _winController;
    
    private List<PlayableSquare> _playableSquares;
    private Dictionary<int, Node> _nodes;

    private LevelSettings _levelSettings;
    private Node _currentNode;
    private PlayableSquare _currentPlayableSquare;
    private List<Node> _availableNodes;

    private void Awake()
    {
        InitializeAllFields();
    }

    private void InitializeAllFields()
    {
        _levelSettings = _fileReader.GetLevelSettings();
        _graphController.Initialize(_levelSettings);

        _nodes = _graphController.GetNodes();
        _playableSquares = _graphController.GetPlayableSquares();

        var finalPositions = _graphController.GetFinalPositions();
        _winController.Initialize(finalPositions);

        _winController.OnWin += DisplayWinMode;
        _moveController.OnGoalAchieved += InvokeOnWinController;

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
        
        if (!_availableNodes.Contains(newPositionNode))
        {
            _currentNode.ChangeColor(ColorProvider.BlockedNode);
            _currentPlayableSquare = null;
            return;
        }
        
        _currentNode = newPositionNode;
        var startNode = _currentPlayableSquare.GetCurrentNode();
        var path = _pathFinder.GetPath(newPositionNode, startNode);

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

    private void InvokeOnWinController()
    {
        _winController.CheckPlayableSquaresPositions(_playableSquares);
    }

    private void DisplayWinMode()
    {
        _mainCanvas.SetActive(false);
        _winCanvas.SetActive(true);
    }

    private void OnDestroy()
    {
        _winController.OnWin -= DisplayWinMode;
        _moveController.OnGoalAchieved -= InvokeOnWinController;
        
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