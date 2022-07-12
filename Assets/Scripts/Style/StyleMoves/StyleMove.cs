using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StyleMove
{
    private SnakeHead _target;
    private StylePointsCounter _stylePointsCounter;
    
    public StyleMove(SnakeHead target, StylePointsCounter stylePointsCounter)
    {
        _target = target;
        _stylePointsCounter = stylePointsCounter;
        
        target.OnSnakeNewMoveStart += ProcessSnakeStep;
    }

    private void ProcessSnakeStep()
    {
        var snakeMovePoints = GetSnakeStylePointsForStep(_target);
        if (snakeMovePoints > 0)
            _stylePointsCounter.IncreasePoints((uint)snakeMovePoints, true);
    }

    protected abstract int GetSnakeStylePointsForStep(SnakeHead snakeHead);
}