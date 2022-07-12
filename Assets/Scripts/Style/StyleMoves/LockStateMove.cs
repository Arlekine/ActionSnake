using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockStateMove : StyleMove
{
    private int _statePoints;
    private int _stateMultiplier;
    
    public LockStateMove(SnakeHead target, StylePointsCounter stylePointsCounter, int statePoints, int stateMultiplier) : base(target, stylePointsCounter)
    {
        _stateMultiplier = stateMultiplier;
        _statePoints = statePoints;
    }

    protected override int GetSnakeStylePointsForStep(SnakeHead snakeHead)
    {
        var field = snakeHead.Field;

        bool isLockedState = false;
        var fieldCellsAmount = field.AllCells.Count * field.AllCells[0].Count;
        var cellsToCheck = new List<Cell>();
        var checkedCells = new List<Cell>();

        cellsToCheck.Add(snakeHead.DestinationCell);
        while (checkedCells.Count < fieldCellsAmount)
        {
            if (cellsToCheck.Count == 0)
            {
                Debug.Log("Lock state");
                isLockedState = checkedCells.Count < fieldCellsAmount * 0.5f;
                break;
            }

            var cellNeighbors = field.GetCellNeighbors(cellsToCheck[0]);

            foreach (var neighbor in cellNeighbors)
            {
                if (neighbor.IsCellFree && checkedCells.Contains(neighbor) == false && cellsToCheck.Contains(neighbor) == false)
                    cellsToCheck.Add(neighbor);
                else if (checkedCells.Contains(neighbor) == false)
                    checkedCells.Add(neighbor);
            }
            
            if (checkedCells.Contains(cellsToCheck[0]) == false)
                checkedCells.Add(cellsToCheck[0]);
            
            cellsToCheck.RemoveAt(0);
        }
            
        Debug.Log(checkedCells.Count + " - " + fieldCellsAmount);
        return isLockedState ? _statePoints : 0;
    }
}