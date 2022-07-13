using System;

public class TightMove : StyleMove
{
    private int _pointsPerHazard = 100;

    public TightMove(SnakeHead target, StylePointsCounter stylePointsCounter, int pointsPerHazard) 
        : base(target, stylePointsCounter)
    {
        _pointsPerHazard = pointsPerHazard;
    }

    protected override int GetSnakeStylePointsForStep(SnakeHead snakeHead)
    {
        var resultPoints = 0;
        var field = snakeHead.Field;
        var sideDirections = MoveDirection.GetSideDirections(snakeHead.CurrentDirection);
        
        for (int i = 0; i < sideDirections.Length; i++)
        {
            var discoverCell = snakeHead.DestinationCell;
            var nextCell = discoverCell + sideDirections[i];
        
            while (field.IsCoordinatesInField(nextCell))
            {
                var nextCellHasHazard = field.IsCellFree(nextCell) == false && field.GetCellContent(nextCell) is SnakeTail;
        
                if (nextCellHasHazard)
                    resultPoints += _pointsPerHazard;
                else
                    break;
                
                nextCell = nextCell + sideDirections[i];
            }
        }

        return resultPoints;
    }
}