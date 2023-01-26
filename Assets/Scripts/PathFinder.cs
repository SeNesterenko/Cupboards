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

    public List<Node> GetPath(Node destination, Node startNode)
    {
        var path = new List<Node> { destination };

        var currentNode = destination;
        var parentNode = _path[currentNode];

        while (parentNode != null)
        {
            path.Add(parentNode);

            currentNode = parentNode;
            
            if (CheckParentNodeEqualToCurrentNode(startNode, parentNode, path)) break;
            
            parentNode = _path[currentNode];

            if (CheckParentNodeEqualToCurrentNode(startNode, parentNode, path)) break;
        }

        path.Reverse();
        return path;
    }

    private static bool CheckParentNodeEqualToCurrentNode(Node startNode, Node parentNode, List<Node> path)
    {
        if (parentNode == startNode)
        {
            path.Add(parentNode);
            return true;
        }

        return false;
    }

    private void DepthFirstSearch(Node previous, Node currentNode)
    {
        if (_availableNodes.Contains(currentNode))
        {
            return;
        }

        if (previous != null)
        {
            _path[currentNode] = previous;
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