using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [SerializeField] private float _duration = 2f;
    
    private float _currentTime;
    private bool _isNextNodeFound;
    
    private Node _startNode;
    private Node _nextNode;
    private Node _destinationNode;

    private PlayableSquare _playableSquare;
    private Dictionary<Node, Node> _path;

    private void Awake()
    {
        enabled = false;
    }

    public void ShowPath(PlayableSquare playableSquare, Dictionary<Node, Node> path, Node startNode, Node destinationNode)
    {
        _path = path;

        _destinationNode = destinationNode;
        _startNode = startNode;
        _nextNode = path[_startNode];
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

        if (_playableSquare.Position.position == _nextNode.Position.position &&
            _playableSquare.Position.position == _destinationNode.Position.position)
        {
            _playableSquare.SetPosition(_nextNode);
            Reset();
        }
        
        if (_playableSquare.Position.position == _nextNode.Position.position)
        {
            foreach (var keyValuePair in _path)
            {
                if (keyValuePair.Key == _nextNode)
                {
                    _currentTime = 0;
                    _startNode = _nextNode;
                    _nextNode = _path[_startNode];
                    _isNextNodeFound = true;
                    break;
                }

                _isNextNodeFound = false;
            }

            if (!_isNextNodeFound)
            {
                _playableSquare.SetPosition(_nextNode);
                Reset();
            }
        }
    }

    private void Reset()
    {
        _startNode = null;
        _nextNode = null;
        _playableSquare = null;
        _path = null;

        enabled = false;
    }
}