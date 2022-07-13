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
        
        var previousPossibleCell = snakeHead.StartCell + snakeHead.PreviousDirection;
        
        if (snakeHead.Field.IsCoordinatesInField(previousPossibleCell))
        {
            possibleDeadAHead = snakeHead.Field.IsCellFree(previousPossibleCell) == false && snakeHead.Field.GetCellContent(previousPossibleCell) is ISnakeHazard;
        }
        
        return possibleDeadAHead ? _pointsForMove : 0;
    }
}