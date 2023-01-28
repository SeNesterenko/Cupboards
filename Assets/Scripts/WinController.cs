using System;
using System.Collections.Generic;
using UnityEngine;

public class WinController : MonoBehaviour
{
    public Action OnWin;
    private List<ConnectionNodesAndPlayableSquares> _finalPositions;

    public void Initialize(List<ConnectionNodesAndPlayableSquares> finalPositions)
    {
        _finalPositions = finalPositions;
    }

    public void CheckPlayableSquaresPositions(List<PlayableSquare> playableSquares)
    {
        var quantityOfMatches = 0;
        
        for (var i = 0; i < playableSquares.Count; i++)
        {
            var currentNode = playableSquares[i].GetCurrentNode();

            if (playableSquares[i] == _finalPositions[i].PlayableSquare &&
                currentNode == _finalPositions[i].Node)
            {
                quantityOfMatches++;
            }
        }

        if (quantityOfMatches == _finalPositions.Count)
        {
            OnWin?.Invoke();
        }
    }
}