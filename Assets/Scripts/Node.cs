using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Action<Node> OnSelected;
    public bool IsBusy;
    public RectTransform Position { get; private set; }

    [SerializeField] private List<Node> _neighbours;

    private Image _color;

    private void Awake()
    {
        Position = GetComponent<RectTransform>();
        _color = GetComponent<Image>();
    }

    public void SetPosition(int x, int y)
    {
        var position = new Vector2(x, y);
        Position.anchoredPosition = position;
        Reset();
    }

    public void AddNeighbour(Node node)
    {
        _neighbours.Add(node);
    }

    public List<Node> GetNeighbours()
    {
        return _neighbours;
    }

    public void ChangeColor(Color color)
    {
        _color.color = color;
    }

    public void SelectNode()
    {
        OnSelected?.Invoke(this);
    }
    
    public void Reset()
    {
        _color.color = ColorProvider.DefaultNode;
    }
}