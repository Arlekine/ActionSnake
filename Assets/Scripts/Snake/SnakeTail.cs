using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : SnakeMover, IDestructible, ISnakeHazard
{
    public event Action<SnakeTail> OnDeathCollision;
    
    public void Cut()
    {
        DestroyImmediate(this.gameObject);
    }

    [EditorButton]
    public void Die()
    {
        OnDeathCollision?.Invoke(this);
    }
}