using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldObjectsSpawner : MonoBehaviour
{
    private Field _field;
    private Dictionary<CellContent, CellCoordinates> OccupiedCells = new Dictionary<CellContent, CellCoordinates>();
    
    public FieldObjectsSpawner(Field field)
    {
        _field = field;
    }

    public CellContent CreateCellContent(CellContent prefab, CellCoordinates cell)
    {
        var newContent = Instantiate(prefab, _field[cell], Quaternion.identity);

        newContent.OnDestroyed += ContentDestroyed;
        if (newContent is MovingCellContent movingContent)
        {
            movingContent.OnMovedToNewCoordinates += ChangeOccupiedCell;
        }

        return newContent;
    }

    private void ChangeOccupiedCell(MovingCellContent content, CellCoordinates newCell)
    {
        if (OccupiedCells.ContainsKey(content) == false)
            throw new ArgumentException("Attempt to move content that is not on field");

        _field.FreeCell(OccupiedCells[content], content);
        _field.OccupyCell(newCell, content);
        
        OccupiedCells[content] = newCell;
    }

    private void ContentDestroyed(CellContent content)
    {
        if (OccupiedCells.ContainsKey(content) == false)
            throw new ArgumentException("Attempt to move content that is not on field");

        content.OnDestroyed -= ContentDestroyed;
        if (content is MovingCellContent movingContent)
        {
            movingContent.OnMovedToNewCoordinates -= ChangeOccupiedCell;
        }
        
        _field.FreeCell(OccupiedCells[content], content);
        OccupiedCells.Remove(content);
    }
}