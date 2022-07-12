using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeStepTime
{
    public static event Action OnStepComplete;
    
    private SnakeHead _snakeHead;

    public SnakeStepTime(SnakeHead snakeHead)
    {
        _snakeHead = snakeHead;
        _snakeHead.OnSnakeNewMoveStart += InvokeTimeStep;
    }

    private void InvokeTimeStep()
    {
        OnStepComplete?.Invoke();
    }
}
