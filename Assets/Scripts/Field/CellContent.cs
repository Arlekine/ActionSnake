using System;
using UnityEngine;

public abstract class CellContent : MonoBehaviour
{
    public event Action<CellContent> OnDestroyed;

    protected void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }
}