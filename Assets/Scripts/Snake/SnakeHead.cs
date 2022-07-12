using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeHead : SnakeMover, IDestructible
{
    public event Action OnSnakeNewMoveStart;
    
    [SerializeField] private float _timeForStep;
    [SerializeField] private int _minimumLength;
    [SerializeField] private SnakeTail _snakeTailPrefab;

    private List<SnakeTail> _tails = new List<SnakeTail>();

    public int Length;// => _tails.Count;
    
    public override void Init(Field field, Cell startCell, MoveDirection startDirection)
    {
        base.Init(field, startCell, startDirection);
        Speed = field.GetCellsSize() / _timeForStep;
    }

    [EditorButton]
    public void Activate()
    {
        enabled = true;

        foreach (var tail in _tails)
        {
            tail.enabled = true;
        }
    }
    
    [EditorButton]
    public void Stop()
    {
        enabled = false;

        foreach (var tail in _tails)
        {
            tail.enabled = false;
        }
    }

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentDirection != MoveDirection.Right)
            SetNewDirection(MoveDirection.Left);
        
        if (Input.GetKeyDown(KeyCode.RightArrow) && _currentDirection != MoveDirection.Left)
            SetNewDirection(MoveDirection.Right);
        
        if (Input.GetKeyDown(KeyCode.UpArrow) && _currentDirection != MoveDirection.Down)
            SetNewDirection(MoveDirection.Up);
        
        if (Input.GetKeyDown(KeyCode.DownArrow) && _currentDirection != MoveDirection.Up)
            SetNewDirection(MoveDirection.Down);
        
        if (Input.GetKeyDown(KeyCode.Space))
            AddTail();

        Move();
        Length = _tails.Count;
    }

    private void OnDestroy()
    {
        foreach (var tail in _tails)
        {
            tail.OnDeathCollision -= CutTailsFromTarget;
        }
    }

    protected override void Move()
    {
        if ((_destinationCell.CenterWorldPosition - transform.position).sqrMagnitude <= Speed * Time.deltaTime)
        {
            StartNextMove(_destinationCell, _nextDirection);
            OnSnakeNewMoveStart?.Invoke();
            return;
        }
        
        base.Move();
    }

    private void CutTailsFromTarget(SnakeTail target)
    {
        if (!_tails.Contains(target))
            throw new ArgumentException("Attempt to cut tail that don't belongs to this snake");

        var targetIndex = _tails.IndexOf(target);
        int tailsAmountToCut = _tails.Count - targetIndex;
        CutTails(tailsAmountToCut);
    }

    public void AddTail()
    {
        var newTail = Instantiate(_snakeTailPrefab);
        SnakeMover prevMover = _tails.Count > 0 ? (SnakeMover)_tails.Last() : this;
        
        newTail.enabled = false;
        newTail.Speed = Speed;
        
        newTail.Init(_field, _field.GetCellAtDirection(prevMover.StartCell, MoveDirection.GetOppositeDirection(prevMover.CurrentDirection)), prevMover.CurrentDirection);
        newTail.OnDeathCollision += CutTailsFromTarget;

        _tails.Add(newTail);
    }

    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public int GetTailIndex(SnakeTail tail)
    {
        if (_tails.Contains(tail) == false)
            throw new ArgumentException("Snake doesn't contain this tail");

        return _tails.IndexOf(tail);
    }

    public override void StartNextMove(Cell startCell, MoveDirection direction)
    {
        for (int i = _tails.Count - 1; i >= 0; i--)
        {
            SnakeMover prevMover = i > 0 ? (SnakeMover)_tails[i - 1] : this;

            _tails[i].enabled = true;
            _tails[i].StartNextMove(prevMover.StartCell, prevMover.CurrentDirection);
        }
        
        base.StartNextMove(startCell, direction);
    }

    public void TeleportToCell(Cell cell)
    {
        transform.position = cell.CenterWorldPosition;
        StartNextMove(cell, _nextDirection);
    }

    [EditorButton]
    public void CutTails(int tailsToCut)
    {
        if (tailsToCut > (_tails.Count - _minimumLength))
            throw new ArgumentException("Attempt to cut more tails than snake has");

        int cutStartIndex = _tails.Count - tailsToCut;
        for (int i = cutStartIndex; i < _tails.Count; i++)
        {
            _tails[i].OnDeathCollision -= CutTailsFromTarget;
            _tails[i].Cut();
        }
        
        _tails.RemoveRange(cutStartIndex, tailsToCut);
    }
}