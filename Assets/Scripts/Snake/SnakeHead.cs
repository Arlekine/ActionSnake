using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SnakeHead : SnakeMover, IDestructible
{
    public event Action OnSnakeNewMoveStart;
    
    [SerializeField] private float _initailTimeForStep;
    [SerializeField] private int _minimumLength;
    [SerializeField] private SnakeTail _snakeTailPrefab;

    private List<SnakeTail> _tails = new List<SnakeTail>();

    public int Length => _tails.Count;
    
    public override void Init(Field field, CellCoordinates startCell, MoveDirection startDirection)
    {
        base.Init(field, startCell, startDirection);

        TimeForStep = _initailTimeForStep;
        _mover.OnMoveComplete += StartNextMove;
        StartMove(startCell, startDirection);
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

    protected void Update()
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
    }

    private void OnDestroy()
    {
        foreach (var tail in _tails)
        {
            tail.OnDeathCollision -= CutTailsFromTarget;
        }
    }

    private void CutTailsFromTarget(SnakeTail target)
    {
        if (!_tails.Contains(target))
            throw new ArgumentException("Attempt to cut tail that don't belongs to this snake");

        var targetIndex = _tails.IndexOf(target);
        int tailsAmountToCut = _tails.Count - targetIndex;
        CutTails(tailsAmountToCut);
    }
    
    private void StartNextMove()
    {
        StartMove(_destinationCell, _nextDirection);
        OnSnakeNewMoveStart?.Invoke();
    }

    public void AddTail()
    {
        var newTail = Instantiate(_snakeTailPrefab);
        SnakeMover lastTail = _tails.Count > 0 ? (SnakeMover)_tails.Last() : this;
        
        newTail.enabled = false;
        newTail.TimeForStep = TimeForStep;
        
        newTail.Init(_field, lastTail.StartCell + MoveDirection.GetOppositeDirection(lastTail.CurrentDirection), lastTail.CurrentDirection);
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

    public override void StartMove(CellCoordinates startCell, MoveDirection direction)
    {
        for (int i = _tails.Count - 1; i >= 0; i--)
        {
            SnakeMover prevMover = i > 0 ? (SnakeMover)_tails[i - 1] : this;

            _tails[i].enabled = true;
            _tails[i].StartMove(prevMover.StartCell, prevMover.CurrentDirection);
        }
        
        base.StartMove(startCell, direction);
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