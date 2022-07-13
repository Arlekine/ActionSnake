using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockStateMove : StyleMove
{
    private enum CellCheckingState
    {
        Unchecked,
        InCheckingQueue,
        Checked
    }
    
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
        var fieldCellsAmount = field.Width * field.Height;

        var checkedCells = new CellCheckingState[field.Width, field.Height];
        var cellsToCheck = new Queue<CellCoordinates>();

        var checkedCellsAmount = 0;
        
        cellsToCheck.Enqueue(snakeHead.DestinationCell);
        
        while (checkedCellsAmount < fieldCellsAmount)
        {
            if (cellsToCheck.Count == 0)
            {
                isLockedState = checkedCellsAmount < fieldCellsAmount * 0.5f;
                break;
            }

            var cellToCheck = cellsToCheck.Dequeue();
            var cellNeighbors = field.GetCellNeighbors(cellToCheck);
        
            foreach (var neighbor in cellNeighbors)
            {
                if (field.IsCellFree(neighbor) && checkedCells[neighbor.X, neighbor.Y] == CellCheckingState.Unchecked)
                {
                    cellsToCheck.Enqueue(neighbor);
                    checkedCells[neighbor.X, neighbor.Y] = CellCheckingState.InCheckingQueue;

                }
                else if (checkedCells[neighbor.X, neighbor.Y] == CellCheckingState.Unchecked)
                {
                    checkedCellsAmount++;
                    checkedCells[neighbor.X, neighbor.Y] = CellCheckingState.Checked;
                }
            }
            
            if (checkedCells[cellToCheck.X, cellToCheck.Y] != CellCheckingState.Checked)
            {
                checkedCells[cellToCheck.X, cellToCheck.Y] = CellCheckingState.Checked;
                checkedCellsAmount++;
            }
        }
        
        return isLockedState ? _statePoints : 0;
    }
}