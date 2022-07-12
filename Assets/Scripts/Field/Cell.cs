using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Action<Cell> OnCellOccupied;
    public Action<Cell> OnCellFreed;
    public Vector3 CenterWorldPosition => _centerWorldPosition;

    public bool IsCellFree => _currentContent == null;

    public ICellContent CellContent => _currentContent;

    public void OccupyCell(ICellContent content)
    {
        _currentContent = content;
        OnCellOccupied?.Invoke(this);
    }

    public void FreeCell(ICellContent content)
    {
        if (_currentContent == content)
        {
            _currentContent = null;
            OnCellFreed?.Invoke(this);
        }
    }

    private ICellContent _currentContent;
    private Vector3 _centerWorldPosition;

    public Cell(Vector3 centerWorldPosition)
    {
        _centerWorldPosition = centerWorldPosition;
    }
}