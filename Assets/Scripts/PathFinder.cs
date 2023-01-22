using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private List<Node> _availableNodes;
    private Dictionary<Node, Node> _path;

    public List<Node> StartSearch(Node startNode)
    {
        _availableNodes = new List<Node>();
        _path = new Dictionary<Node, Node>();

        DepthFirstSearch(null, startNode);
        ChangeFreeNodesColor();
        
        return _availableNodes;
    }

    public Dictionary<Node, Node> GetPath()
    {
        return _path;
    }

    private void DepthFirstSearch(Node previous, Node currentNode)
    {
        if (_availableNodes.Contains(currentNode))
        {
            return;
        }

        if (previous != null)
        {
            _path[previous] = currentNode;
        }
        
        _availableNodes.Add(currentNode);

        foreach (var neighbour in currentNode.GetNeighbours().Where(neighbour => !neighbour.IsBusy))
        {
            DepthFirstSearch(currentNode, neighbour);
        }
    }

    private void ChangeFreeNodesColor()
    {
        for (var i = 1; i < _availableNodes.Count; i++)
        {
            _availableNodes[i].ChangeColor(ColorProvider.AvailableNode);
        }
    }
}