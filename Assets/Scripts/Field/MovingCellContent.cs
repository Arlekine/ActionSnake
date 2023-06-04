using System;

public abstract class MovingCellContent : CellContent
{
    public event Action<MovingCellContent, CellCoordinates> OnMovedToNewCoordinates;
    

    public void StartMove(CellCoordinates startCell, MoveDirection direction)
    {
        transform.position = _field[startCell];
        _startCell = startCell;

        _destinationCell = startCell + direction;
        
        _previousDirection = _currentDirection;
        _currentDirection = direction;
        
        _mover.Move(_field[_startCell], _field[_destinationCell], TimeForStep);
    }


    protected abstract void Move(CellCoordinates startCell, MoveDirection direction);
}