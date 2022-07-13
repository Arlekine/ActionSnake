using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeTail : SnakeMover, IDestructible, ISnakeHazard
{
    public event Action<SnakeTail> OnDeathCollision;
    
    public void Cut()
    {
        Destroy(gameObject);
    }

    [EditorButton]
    public void Die()
    {
        OnDeathCollision?.Invoke(this);
    }
}