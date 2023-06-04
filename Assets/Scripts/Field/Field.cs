using System;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    private class Cell
    {
        public Vector3 CenterWorldPosition => _centerWorldPosition;

        public bool IsCellFree => _currentContent == null;

        public CellContent CellContent => _currentContent;

        public void OccupyCell(CellContent content)
        {
            _currentContent = content;
        }

        public bool FreeCell(CellContent content)
        {
            if (_currentContent == content)
            {
                _currentContent = null;
                return true;
            }

            return false;
        }

        private CellContent _currentContent;
        private Vector3 _centerWorldPosition;

        public Cell(Vector3 centerWorldPosition)
        {
            _centerWorldPosition = centerWorldPosition;
        }
    }

    public Action<CellCoordinates, CellContent> OnCellOccupied;
    public Action<CellCoordinates> OnCellFree;
    
    private List<List<Cell>> _allCells = new List<List<Cell>>();
    private List<Cell> _occupiedCells = new List<Cell>();
    private List<Cell> _freeCells = new List<Cell>();

    private float _cellSize;

    //public List<List<Cell>> AllCells => _allCells;

    public int Width => _allCells.Count;
    public int Height => _allCells[0].Count;

    public float CellS => _cellSize;

    public Vector3 this[CellCoordinates coordinates] => this[coordinates.X, coordinates.Y];

    public Vector3 this[int x, int y]
    {
        get
        {
            if (IsCoordinatesInField(x, y) == false)
                throw new ArgumentException("Attempt to get cell out of field");

            return _allCells[x][y].CenterWorldPosition;
        }
    }
    
    public Field(int width, int height, float cellSize, Vector3 worldFieldStartPosition)
    {
        _cellSize = cellSize;
        
        for (int x = 0; x < width; x++)
        {
            _allCells.Add(new List<Cell>());
            for (int y = 0; y < height; y++)
            {
                var cellCenter = worldFieldStartPosition + new Vector3(cellSize * x, 0f, cellSize * y);
                var newCell = new Cell(cellCenter);
                
                _allCells[x].Add(newCell);
            }
        }
    }

    private bool IsCoordinatesOutOfField(int x, int y) =>
        x < 0 || x >= _allCells.Count || y < 0 || y >= _allCells[0].Count;

    private bool IsAreaAroundCellFree(int x, int y, int areaSize)
    {
        if (areaSize <= 0)
            throw new ArgumentException("Area size should be greater than zero");
        
        if (IsCoordinatesOutOfField(x, y))
            throw new ArgumentException("Invalid coordinates for cell");

        bool isAreaFree = true;

        for (int currentAreaSize = 1; currentAreaSize <= areaSize; currentAreaSize++)
        {
            for (int xOffset = -currentAreaSize; xOffset <= currentAreaSize; xOffset++)
            {
                for (int yOffset = -currentAreaSize; yOffset <= currentAreaSize; yOffset++)
                {
                    if (IsCoordinatesOutOfField(x + xOffset, y + yOffset))
                        continue;
                    
                    if (_allCells[x + xOffset][y + yOffset].IsCellFree == false)
                    {
                        isAreaFree = false;
                        break;
                    }
                }
            }
        }

        return isAreaFree;
    }

    public bool IsCoordinatesInField(CellCoordinates coordinates)
    {
        return IsCoordinatesInField(coordinates.X, coordinates.Y);
    }
    
    public bool IsCoordinatesInField(int x, int y)
    {
        return x >= 0 && y >= 0 && x < _allCells.Count && y < _allCells[0].Count;
    }

    public List<CellCoordinates> GetFreeCells(int freeArea = 0)
    {
        var freeCells = new List<CellCoordinates>();
        
        for (int x = 0; x < _allCells.Count; x++)
        {
            for (int y = 0; y < _allCells[x].Count; y++)
            {
                if (_allCells[x][y].IsCellFree)
                {
                    if (freeArea == 0 || IsAreaAroundCellFree(x, y, freeArea))
                        freeCells.Add(new CellCoordinates(x, y));
                }
            }
        }

        return freeCells;
    }

    public List<CellCoordinates> GetCellNeighbors(CellCoordinates coordinates)
    {
        var neighbors = new List<CellCoordinates>();
        var directions = MoveDirection.GetAllPossibleDirections();

        foreach (var direction in directions)
        {
            var checkCell = coordinates + direction;
            if (IsCoordinatesInField(checkCell))
                neighbors.Add(checkCell);
        }

        return neighbors;
    }

    public bool IsCellFree(CellCoordinates coordinates)
    {
        return _allCells[coordinates.X][coordinates.Y].IsCellFree;
    }
    
    public CellContent GetCellContent(CellCoordinates coordinates)
    {
        return _allCells[coordinates.X][coordinates.Y].CellContent;
    }

    public void OccupyCell(CellCoordinates coordinates, CellContent cellContent)
    {
        _allCells[coordinates.X][coordinates.Y].OccupyCell(cellContent);
        OnCellOccupied?.Invoke(coordinates, cellContent);
        //TODO: add removing cell content from another cell
    }

    public void FreeCell(CellCoordinates coordinates, CellContent contentToFreeFrom)
    {
        if (_allCells[coordinates.X][coordinates.Y].FreeCell(contentToFreeFrom))
            OnCellFree?.Invoke(coordinates);
        //TODO: add checks
    }

    public void FreeCell(CellContent content)
    {
        foreach (var cellList in _allCells)
        {
            foreach (var cell in cellList)
            {
                if (cell.IsCellFree == false && cell.CellContent == content)
                {
                    cell.FreeCell(content);
                    return;
                }
            }
        }
    }
}
