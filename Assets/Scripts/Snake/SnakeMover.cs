using UnityEngine;

public class SnakeMover : MonoBehaviour, ICellContent
{
    public Cell StartCell => _startCell;
    public Cell DestinationCell => _destinationCell;
    public MoveDirection CurrentDirection => _currentDirection;
    public MoveDirection PreviousDirection => _previousDirection;
    public Field Field => _field;

    public float Speed
    {
        get => _speed;
        set => _speed = Mathf.Max(value, 0);
    }

    private float _speed;
    protected Field _field;
    protected Cell _destinationCell;
    protected Cell _startCell;
    protected MoveDirection _previousDirection;
    protected MoveDirection _currentDirection;
    protected MoveDirection _nextDirection;

    public virtual void Init(Field field, Cell startCell, MoveDirection startDirection)
    {
        _field = field;
        _startCell = startCell;
        _currentDirection = startDirection;
        
        _nextDirection = startDirection;
        _destinationCell = _field.GetCellAtDirection(_startCell, _currentDirection);
        
        transform.position = startCell.CenterWorldPosition;
    }
    
    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        var currentDirectionVector = _destinationCell.CenterWorldPosition - _startCell.CenterWorldPosition;
        currentDirectionVector = currentDirectionVector.normalized;
        
        transform.position += currentDirectionVector * (Speed * Time.deltaTime);
    }

    public void SetNewDirection(MoveDirection direction)
    {
        _nextDirection = direction;
    }

    public virtual void StartNextMove(Cell startCell, MoveDirection direction)
    {
        transform.position = startCell.CenterWorldPosition;
        _startCell = startCell;
        
        _destinationCell.FreeCell(this);
        
        _destinationCell = _field.GetCellAtDirection(startCell, direction);
        
        _destinationCell.OccupyCell(this);

        _previousDirection = _currentDirection;
        _currentDirection = direction;
    }
}