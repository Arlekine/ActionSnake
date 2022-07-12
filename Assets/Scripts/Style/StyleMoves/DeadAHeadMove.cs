public class DeadAHeadMove : StyleMove
{
    private int _pointsForMove;

    public DeadAHeadMove(SnakeHead target, StylePointsCounter stylePointsCounter, int pointsForMove) : base(target, stylePointsCounter)
    {
        _pointsForMove = pointsForMove;
    }

    protected override int GetSnakeStylePointsForStep(SnakeHead snakeHead)
    {
        bool possibleDeadAHead = false;
        if (snakeHead.Field.HasCellAtDirection(snakeHead.StartCell, snakeHead.PreviousDirection))
        {
            var targetCellBeforeMoveChange = snakeHead.Field.GetCellAtDirection(snakeHead.StartCell, snakeHead.PreviousDirection);
            possibleDeadAHead = targetCellBeforeMoveChange.IsCellFree == false && targetCellBeforeMoveChange.CellContent is ISnakeHazard;
        }

        return possibleDeadAHead ? _pointsForMove : 0;
    }
}