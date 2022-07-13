using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class FieldObject : MonoBehaviour, ICellContent
{
    public Action<FieldObject> OnObjectDestroyed;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        var snake = other.GetComponent<SnakeHead>();
        if (snake != null)
        {
            InteractWithSnake(snake);
        }
    }

    protected virtual void OnDestroy()
    {
        OnObjectDestroyed?.Invoke(this);
    }

    protected abstract void InteractWithSnake(SnakeHead snake);
}