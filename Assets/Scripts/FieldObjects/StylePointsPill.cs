using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StylePointsPill : FieldObject
{ 
    [SerializeField] private int _pointsForPill = 500;
    
    private StylePointsCounter _stylePointsCounter;

    public void Init(StylePointsCounter stylePointsCounter)
    {
        _stylePointsCounter = stylePointsCounter;
    }

    protected override void InteractWithSnake(SnakeHead snake)
    {
        _stylePointsCounter.IncreasePoints((uint)_pointsForPill, false);
        Destroy(gameObject);
    }
}