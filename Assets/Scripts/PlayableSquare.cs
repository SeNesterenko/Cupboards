using System;
using UnityEngine;

public class PlayableSquare : MonoBehaviour
{
    public Action<Node, PlayableSquare> OnSelected;
    
    public RectTransform Position { get; set; }
    
    [SerializeField] private Node _node;

    private void Awake()
    {
        Position = GetComponent<RectTransform>();
    }

    public void SetPosition(Node node)
    {
        if (_node != null)
        {
            _node.IsBusy = false;
        }

        _node = node;
        _node.IsBusy = true;
    }
    
    public void SelectPlayableSquare()
    {
        OnSelected?.Invoke(_node, this);
    }

    public Node GetCurrentNode()
    {
        return _node;
    }
}