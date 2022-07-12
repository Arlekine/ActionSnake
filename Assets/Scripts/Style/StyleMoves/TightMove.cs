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

            while (field.HasCellAtDirection(discoverCell, sideDirections[i]))
            {
                if (field.HasCellAtDirection(discoverCell, sideDirections[i]) == false)
                    break;

                var nextCell = field.GetCellAtDirection(discoverCell, sideDirections[i]);
                var nextCellHasHazard = nextCell.IsCellFree == false && nextCell.CellContent is SnakeTail;

                if (nextCellHasHazard)
                    resultPoints += _pointsPerHazard;
                else
                    break;

                discoverCell = nextCell;
            }
        }

        return resultPoints;
    }
}