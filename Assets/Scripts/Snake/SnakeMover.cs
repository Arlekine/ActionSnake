using System;
using UnityEngine;

[RequireComponent(typeof(Mover))]
public class SnakeMover : MovingCellContent
{
    public CellCoordinates StartCell => _startCell;
    public CellCoordinates DestinationCell => _destinationCell;
    public MoveDirection CurrentDirection => _currentDirection;
    public MoveDirection PreviousDirection => _previousDirection;
    public Field Field => _field;

    public float TimeForStep
    {
        get => _timeForStep;
        set
        {
            if (value <= 0)
                throw new ArgumentException("SnakeMover moveTime cannot be zero or less");

            _timeForStep = value;
        }
    }

    protected Field _field;
    protected CellCoordinates _destinationCell;
    protected CellCoordinates _startCell;
    protected MoveDirection _previousDirection;
    protected MoveDirection _currentDirection;
    protected MoveDirection _nextDirection;
    protected Mover _mover;

    private float _timeForStep;

    public virtual void Init(Field field, CellCoordinates startCell, MoveDirection startDirection)
    {
        _mover = GetComponent<Mover>();

        _field = field;
        _startCell = startCell;
        _currentDirection = startDirection;
        
        _nextDirection = startDirection;
        _destinationCell = startCell + startDirection;
        
        transform.position = field[startCell];
    }

    public void SetNewDirection(MoveDirection direction)
    {
        _nextDirection = direction;
    }

    public void TeleportToCell(CellCoordinates cell)
    {
        transform.position = _field[cell];
        StartMove(cell, _nextDirection);
    }

    public virtual void StartMove(CellCoordinates startCell, MoveDirection direction)
    {
        transform.position = _field[startCell];
        _startCell = startCell;

        _destinationCell = startCell + direction;
        
        _previousDirection = _currentDirection;
        _currentDirection = direction;
        
        _mover.Move(_field[_startCell], _field[_destinationCell], TimeForStep);
    }

    private void OnDestroy()
    {
        _field.FreeCell(_destinationCell, this);
    }
}