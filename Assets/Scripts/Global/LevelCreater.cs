using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreater : MonoBehaviour
{
    [SerializeField] private int _cellsWidthCount;
    [SerializeField] private int _cellsHeightCount;
    [SerializeField] private float _cellSize;
    [SerializeField] private float _cameraUIOffset;
    [SerializeField] private Transform _fieldStartPosition;
    [SerializeField] private SnakeHead _snake;
    [SerializeField] private TeleportTrigger _teleportTriggerPrefab;
    [SerializeField] private FreeCellsTester _freeCellsTester;
    [SerializeField] private Camera _camera;

    private Field _field;
    private List<List<GameObject>> _cellPoints = new List<List<GameObject>>();
    
    private void Start()
    {
        _field = new Field(_cellsWidthCount, _cellsHeightCount, _cellSize, _fieldStartPosition.position);

        for (int i = 0; i < _field.AllCells.Count; i++)
        {
            _cellPoints.Add(new List<GameObject>());
            for (int j = 0; j < _field.AllCells[i].Count; j++)
            {
                var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                point.transform.parent = _fieldStartPosition;
                point.transform.position = _field.AllCells[i][j].CenterWorldPosition;
                point.transform.localScale = Vector3.one * 0.1f;

                _cellPoints[i].Add(point);

                // _field.AllCells[i][j].OnCellOccupied += (cell) =>
                // {
                //     var coordinates = _field.GetCellCoordinates(cell);
                //     _cellPoints[coordinates.x][coordinates.y].GetComponent<Renderer>().material.color = Color.red;
                // };
                //
                // _field.AllCells[i][j].OnCellFreed += (cell) =>
                // {
                //     var coordinates = _field.GetCellCoordinates(cell);
                //     _cellPoints[coordinates.x][coordinates.y].GetComponent<Renderer>().material.color = Color.white;
                // };

                if (i == 0 && j != 0)
                    CreateTeleportTrigger(_field.AllCells[i][j].CenterWorldPosition, _field.AllCells[_field.AllCells.Count - 2][j]);
                
                if (i == _field.AllCells.Count - 1 && j != _field.AllCells[i].Count - 1)
                    CreateTeleportTrigger(_field.AllCells[i][j].CenterWorldPosition, _field.AllCells[1][j]);
                
                if (j == 0 && i != 0)
                    CreateTeleportTrigger(_field.AllCells[i][j].CenterWorldPosition, _field.AllCells[i][_field.AllCells[i].Count - 2]);
                
                if (j == _field.AllCells[i].Count - 1  && i != _field.AllCells.Count - 1)
                    CreateTeleportTrigger(_field.AllCells[i][j].CenterWorldPosition, _field.AllCells[i][1]);
            }
        }
        
        _snake.Init(_field, _field.AllCells[1][1], MoveDirection.Right);
        
        var cameraPositioner = new FieldCameraPositioner(_camera, _cameraUIOffset);
        cameraPositioner.PositionCamera(_field);
        var snakeTime = new SnakeStepTime(_snake);
        
        _freeCellsTester.SetCellsList(_field, _cellPoints);
    }


    private void CreateTeleportTrigger(Vector3 spawnPosition, Cell teleportationCell)
    {
        var teleport = Instantiate(_teleportTriggerPrefab, spawnPosition, Quaternion.identity);
        teleport.SetTeleportationCell(teleportationCell);
    }
}