using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayableSquare : MonoBehaviour
{
    public Action<Node, PlayableSquare> OnSelected;
    public RectTransform Position { get; set; }
    
    [SerializeField] private Node _node;

    private Image _background;

    private void Awake()
    {
        Position = GetComponent<RectTransform>();
        _background = GetComponent<Image>();
    }

    public void SetPosition(Node node)
    {
        if (_node != null)
        {
            _node.IsBusy = false;
        }

        _node = node;
        _node.IsBusy = true;
        Position.anchoredPosition = _node.Position.anchoredPosition;
    }

    public void SetColor(Color color)
    {
        _background.color = color;
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