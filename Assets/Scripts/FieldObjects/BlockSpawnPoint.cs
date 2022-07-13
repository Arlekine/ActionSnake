using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawnPoint : FieldObject
{
    [SerializeField] private SnakeDeathTrigger _blockPrefab;
    [SerializeField] private int _blockSpawnTime;
    [SerializeField] private int _blockLifeTime;

    private SnakeStepTimer _currentTimer;
    
    private void Start()
    {
        _currentTimer= new SnakeStepTimer(_blockSpawnTime);
        _currentTimer.OnTimer += SpawnBlock;
        _currentTimer.StartTimer(false);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (_currentTimer != null)
        {
            _currentTimer.OnTimer -= SpawnBlock;
            _currentTimer.OnTimer -= DestroyBlock;
            
            if (_currentTimer.TimerStarted)
                _currentTimer.StopTimer();
        }
    }

    private void SpawnBlock()
    {
        Instantiate(_blockPrefab, transform);
        
        _currentTimer = new SnakeStepTimer(_blockLifeTime);
        _currentTimer.OnTimer += DestroyBlock;
        _currentTimer.StartTimer(false);
    }

    private void DestroyBlock()
    {
        Destroy(gameObject);
    }

    protected override void InteractWithSnake(SnakeHead snake)
    { }
}
