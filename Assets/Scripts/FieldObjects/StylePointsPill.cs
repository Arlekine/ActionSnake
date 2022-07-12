using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylePointsPill : FieldObject
{
    [SerializeField] private StylePointsCounter _stylePointsCounter;
    [SerializeField] private int _pointsForPill = 500;
    
    protected override void InteractWithSnake(SnakeHead snake)
    {
        _stylePointsCounter.IncreasePoints((uint)_pointsForPill, false);
    }
}