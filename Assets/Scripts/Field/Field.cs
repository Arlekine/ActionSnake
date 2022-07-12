using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    public List<List<Cell>> AllCells => _allCells;

    public int Width => _allCells.Count;
    public int Height => _allCells[0].Count;
    
    private List<List<Cell>> _allCells = new List<List<Cell>>();
    private List<Cell> _occupiedCells = new List<Cell>();
    private List<Cell> _freeCells = new List<Cell>();

    private float _cellSize;

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

    public List<Cell> GetFreeCells(int freeArea = 0)
    {
        var freeCells = new List<Cell>();
        
        for (int x = 0; x < _allCells.Count; x++)
        {
            for (int y = 0; y < _allCells[x].Count; y++)
            {
                if (_allCells[x][y].IsCellFree)
                {
                    if (freeArea == 0 || IsAreaAroundCellFree(x, y, freeArea))
                        freeCells.Add(_allCells[x][y]);
                }
            }
        }

        return freeCells;
    }

    public float GetCellsSize()
    {
        return _cellSize;
    }

    public List<Cell> GetCellNeighbors(Cell cell)
    {
        var neighbors = new List<Cell>();
        var directions = MoveDirection.GetAllPossibleDirections();

        foreach (var direction in directions)
        {
            if (HasCellAtDirection(cell, direction))
                neighbors.Add(GetCellAtDirection(cell, direction));
        }

        return neighbors;
    }

    public Vector2Int GetCellCoordinates(Cell cell)
    {
        var cellCoordinates = new Vector2Int();
        cellCoordinates.x = Mathf.RoundToInt((cell.CenterWorldPosition.x - _allCells[0][0].CenterWorldPosition.x ) / _cellSize);
        cellCoordinates.y = Mathf.RoundToInt((cell.CenterWorldPosition.z - _allCells[0][0].CenterWorldPosition.z ) / _cellSize);

        return cellCoordinates;
    }

    public bool HasCellAtDirection(Cell startCell, MoveDirection direction)
    {
        var cellCoordinates = GetCellCoordinates(startCell);
        
        if (_allCells[cellCoordinates.x][cellCoordinates.y] != startCell)
            throw new ArgumentException($"Attempt to get cell that is not presented on field");

        cellCoordinates += direction.FieldCoordinatesDirection;

        return cellCoordinates.x >= 0 && cellCoordinates.y >= 0 && cellCoordinates.x < _allCells.Count && cellCoordinates.y < _allCells[0].Count;
    }

    public Cell GetCellAtDirection(Cell startCell, MoveDirection direction)
    {
        if (HasCellAtDirection(startCell, direction) == false)
            throw new Exception("Attempt to get cell out of field");
        
        var cellCoordinates = GetCellCoordinates(startCell);
        cellCoordinates += direction.FieldCoordinatesDirection;
        
        return _allCells[cellCoordinates.x][cellCoordinates.y];
    }
}
