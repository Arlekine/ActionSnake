public class UroborosMove : StyleMove
{
    private int _pointsPerStep = 100;
    private int _distanceToActivateMove = 6;
    
    public UroborosMove(SnakeHead target, StylePointsCounter stylePointsCounter, int pointsPerStep, int distanceToActivateMove) : base(target, stylePointsCounter)
    {
        _pointsPerStep = pointsPerStep;
        _distanceToActivateMove = distanceToActivateMove;
    }

    protected override int GetSnakeStylePointsForStep(SnakeHead snakeHead)
    {
        var field = snakeHead.Field;
        var nextCell = snakeHead.DestinationCell;
        var pointsToAdd = 0;
        
        for (int i = 0; i < _distanceToActivateMove; i++)
        {
            nextCell = nextCell + snakeHead.CurrentDirection;
            if (snakeHead.Field.IsCoordinatesInField(nextCell) == false)
                break;
        
            if (field.IsCellFree(nextCell) == false)
            {
                if (field.GetCellContent(nextCell) is SnakeTail tail)
                {
                    var isFollowTail = snakeHead.GetTailIndex(tail) == snakeHead.Length - 1 &&
                                       tail.CurrentDirection == snakeHead.CurrentDirection;
        
                    if (isFollowTail)
                        pointsToAdd = _pointsPerStep;
                }
                
                break;
            }
        }

        return pointsToAdd;
    }
}