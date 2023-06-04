using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private int _cellsWidthCount;
    [SerializeField] private int _cellsHeightCount;
    [SerializeField] private float _cellSize;
    [SerializeField] private float _cameraUIOffset;
    [SerializeField] private Transform _fieldStartPosition;
    [SerializeField] private SnakeHead _snake;
    [SerializeField] private TeleportTrigger _teleportTriggerPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private SnakeGrower _snakeGrower;
    [SerializeField] private List<FieldSpawnerBase> _fieldSpawners = new List<FieldSpawnerBase>();

    private Field _field;
    private List<List<GameObject>> _cellPoints = new List<List<GameObject>>();
    
    private void Start()
    {
        _field = new Field(_cellsWidthCount, _cellsHeightCount, _cellSize, _fieldStartPosition.position);

        for (int x = 0; x < _cellsWidthCount; x++)
        {
            _cellPoints.Add(new List<GameObject>());
            for (int y = 0; y < _cellsHeightCount; y++)
            {
                var point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                point.transform.parent = _fieldStartPosition;
                point.transform.position = _field[x, y];
                point.transform.localScale = Vector3.one * 0.1f;

                _cellPoints[x].Add(point);

                if (x == 0 && y != 0)
                    CreateTeleportTrigger(new CellCoordinates(x, y), new CellCoordinates(_field.Width - 2, y));
                
                if (x == _field.Width - 1 && y != _field.Height - 1)
                    CreateTeleportTrigger(new CellCoordinates(x, y), new CellCoordinates(1,y));
                
                if (y == 0 && x != 0)
                    CreateTeleportTrigger(new CellCoordinates(x, y), new CellCoordinates(x, _field.Height - 2));
                
                if (y == _field.Height - 1  && x != _field.Width - 1)
                    CreateTeleportTrigger(new CellCoordinates(x, y), new CellCoordinates(x,1));
            }
        }
        
        _snake.Init(_field, new CellCoordinates(1, 1), MoveDirection.Right);
        
        var cameraPositioner = new FieldCameraPositioner(_camera, _cameraUIOffset);
        cameraPositioner.PositionCamera(_field);
        var snakeTime = new SnakeStepTime(_snake);
        _snakeGrower.Init(_snake);

        foreach (var fieldSpawner in _fieldSpawners)
        {
            fieldSpawner.Init(_field);
        }
    }


    private void CreateTeleportTrigger(CellCoordinates spawnPosition, CellCoordinates teleportationCell)
    {
        var teleport = Instantiate(_teleportTriggerPrefab, _field[spawnPosition], Quaternion.identity);
        teleport.SetTeleportationCell(teleportationCell);
        _field.OccupyCell(spawnPosition, teleport);
    }
}