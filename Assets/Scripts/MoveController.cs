using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Action OnGoalAchieved;
    
    [SerializeField] private float _duration = 2f;
    
    private float _currentTime;
    private int _countIndex;
    private bool _isNextNodeFound;
    
    private Node _startNode;
    private Node _nextNode;
    private Node _destinationNode;

    private PlayableSquare _playableSquare;
    private List<Node> _path;

    private void Awake()
    {
        enabled = false;
    }

    public void PlayPath(PlayableSquare playableSquare, List<Node> path)
    {
        _countIndex = path.Count > 2 ? 1 : 0;
        
        _path = path;
        _destinationNode = path[^1];
        _startNode = path[0];
        _nextNode = path[1];
        _playableSquare = playableSquare;
        
        enabled = true;
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        var progress = _currentTime / _duration;
        var newPosition = Vector2.Lerp(_startNode.Position.position,
            _nextNode.Position.position, progress);

        _playableSquare.Position.position = newPosition;

        if (_playableSquare.Position.position == _destinationNode.Position.position)
        {
            _playableSquare.SetPosition(_destinationNode);
            Reset();
            OnGoalAchieved?.Invoke();
            return;
        }
        
        if (_playableSquare.Position.position == _nextNode.Position.position && _countIndex < _path.Count - 1)
        {
            _countIndex++;
            _currentTime = 0;

            _startNode = _nextNode;
            _nextNode = _path[_countIndex];
        }
    }

    private void Reset()
    {
        _currentTime = 0;
        _startNode = null;
        _nextNode = null;
        _playableSquare = null;
        _path = null;
        _destinationNode = null;

        enabled = false;
    }
}