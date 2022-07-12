using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCellsTester : MonoBehaviour, ICellContent
{
    private List<List<GameObject>> _cellsVisuals;
    private Field _field;

    public void SetCellsList(Field field, List<List<GameObject>> cells)
    {
        _field = field;
        _cellsVisuals = cells;
    }

    [EditorButton]
    private void ShowArea_0_Cells()
    {
        HighlightCells(_field.GetFreeCells());
    }
    
    [EditorButton]
    private void ShowArea_1_Cells()
    {
        HighlightCells(_field.GetFreeCells(1));
    }
    
    [EditorButton]
    private void ShowArea_2_Cells()
    {
        HighlightCells(_field.GetFreeCells(2));
    }
    
    [EditorButton]
    private void ShowArea_3_Cells()
    {
        HighlightCells(_field.GetFreeCells(3));
    }
    
    [EditorButton]
    private void ShowArea_4_Cells()
    {
        HighlightCells(_field.GetFreeCells(4));
    }
    
    [EditorButton]
    private void ShowArea_5_Cells()
    {
        HighlightCells(_field.GetFreeCells(5));
    }

    public void HighlightCells(List<Cell> cellsToHighlight)
    {
        foreach (var cellsList in _cellsVisuals)
        {
            foreach (var cell in cellsList)
            {
                cell.GetComponent<Renderer>().material.color = Color.white;
            }
        }

        foreach (var cell in cellsToHighlight)
        {
            var coordinates = _field.GetCellCoordinates(cell);
            _cellsVisuals[coordinates.x][coordinates.y].GetComponent<Renderer>().material.color = Color.green;
        }
    }

    [EditorButton]
    public void OccupyCell(int x, int y)
    {
        _field.AllCells[x][y].OccupyCell(this); 
        _cellsVisuals[x][y].GetComponent<Renderer>().material.color = Color.red;
    }
}
