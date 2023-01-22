using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PathFinder _pathFinder;
    [SerializeField] private EffectController _effectController;
    
    [SerializeField] private List<PlayableSquare> _playableSquares;
    [SerializeField] private List<Node> _nodes;

    private Node _currentNode;
    private PlayableSquare _currentPlayableSquare;
    private List<Node> _availableNodes;

    private void Awake()
    {
        foreach (var playableSquare in _playableSquares)
        {
            playableSquare.OnSelected += ShowAvailableNodes;
        }

        foreach (var node in _nodes)
        {
            node.OnSelected += ChangePlayableSquarePosition;
        }
    }

    private void ShowAvailableNodes(Node startNode, PlayableSquare playableSquare)
    {
        FullResetNodes();
        
        if (_currentNode != null)
        {
            _currentNode.Reset();
        }

        _currentPlayableSquare = playableSquare;
        _availableNodes = _pathFinder.StartSearch(startNode);
    }

    private void ChangePlayableSquarePosition(Node newPositionNode)
    {
        FullResetNodes();
        
        _currentNode = newPositionNode;
        var path = _pathFinder.GetPath();
        
        if (!_availableNodes.Contains(_currentNode))
        {
            _currentNode.ChangeColor(ColorProvider.BlockedNode);
            _currentPlayableSquare = null;
            return;
        }

        var startNode = _currentPlayableSquare.GetCurrentNode();
        
        if (_currentPlayableSquare != null)
        {
            _effectController.ShowPath(_currentPlayableSquare, path, startNode, _currentNode);
            //_currentPlayableSquare.SetPosition(_currentNode);
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
            node.OnSelected -= ChangePlayableSquarePosition;
        }
    }
}