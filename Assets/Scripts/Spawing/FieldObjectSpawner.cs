using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class FieldObjectSpawner<T> : FieldSpawnerBase where T : FieldObject
{
    [SerializeField] private T _objectPrefab;
    
    [Space]
    [SerializeField] private int _minSpawnTime = 10;
    [SerializeField] private int _maxSpawnTime = 20;
    [SerializeField] private int _maxSpawnedObject = -1;
    [SerializeField] private int _freeAreaSize = 1;

    private Field _field;
    private SnakeStepTimer _currentTimer;
    private List<FieldObject> _spawnedObject = new List<FieldObject>();

    private void OnDestroy()
    {
        if (_currentTimer != null)
        {
            _currentTimer.OnTimer -= SpawnObject;
            
            if (_currentTimer.TimerStarted)
                _currentTimer.StopTimer();
        }
    }

    public override void Init(Field field)
    {
        _field = field;
        RestartTimer();
    }

    private int GetRandomSpawnTime()
    {
        return Random.Range(_minSpawnTime, _maxSpawnTime + 1);
    }

    private void RestartTimer()
    {
        if (_currentTimer != null)
        {
            _currentTimer.OnTimer -= SpawnObject;
            if (_currentTimer.TimerStarted)
                _currentTimer.StopTimer();
        }

        _currentTimer = new SnakeStepTimer(GetRandomSpawnTime());
        _currentTimer.StartTimer(false);
        _currentTimer.OnTimer += SpawnObject;
    }

    private void ClearObjectFormList(FieldObject fieldObject)
    {
        if (_spawnedObject.Contains(fieldObject) == false)
            throw new ArgumentException("Attempt to remove object that not in the list");
        
        fieldObject.OnObjectDestroyed -= ClearObjectFormList;
        _spawnedObject.Remove(fieldObject);
        
        _field.FreeCell(fieldObject);
        
        print(_spawnedObject.Count);
    }

    private void SpawnObject()
    {
        if (_maxSpawnedObject > 0 && _spawnedObject.Count >= _maxSpawnedObject)
        {
            RestartTimer();
            return;
        }

        var possibleSpawnPoints = _field.GetFreeCells(_freeAreaSize);

        if (possibleSpawnPoints.Count == 0)
        {
            RestartTimer();
            return;
        }

        var spawnPointIndex = Random.Range(0, possibleSpawnPoints.Count);
        var spawnPoint = possibleSpawnPoints[spawnPointIndex];

        var newObject = Instantiate(_objectPrefab, _field[spawnPoint], Quaternion.identity);
        InitializeObject(newObject);
        
        _field.OccupyCell(spawnPoint, newObject);

        _spawnedObject.Add(newObject);
        newObject.OnObjectDestroyed += ClearObjectFormList;

        RestartTimer();
        print(_spawnedObject.Count);
    }

    protected abstract void InitializeObject(T fieldObject);
}