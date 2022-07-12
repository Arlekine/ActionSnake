using System;
using UnityEngine;

[Serializable]
public class StyleLevel
{
    public event Action OnIncreaseLevel;
    public event Action OnDecreaseLevel;

    [SerializeField] private uint _multiplier;
    [SerializeField] private uint _pointsToNextLevel;
    [SerializeField] private uint _pointsDecreasingRatePerStep;
    
    public uint Multiplier => _multiplier;
    public uint CurrentPointsProgress { get; private set; }
    public uint PointsToNextLevel => _pointsToNextLevel;

    public void Activate(float startPointsInPercents)
    {
        if (startPointsInPercents < 0 || startPointsInPercents > 1)
            throw new ArgumentException($"{nameof(startPointsInPercents)} - should be between 0 and 1");
        
        CurrentPointsProgress = (uint)(_pointsToNextLevel * startPointsInPercents);
        SnakeStepTime.OnStepComplete += DecreasePoints;
    }

    public void Deactivate()
    {
        SnakeStepTime.OnStepComplete -= DecreasePoints;
    }

    public void IncreasePoints(uint rawPoints)
    {
        CurrentPointsProgress += rawPoints;

        if (CurrentPointsProgress >= PointsToNextLevel)
        {
            CurrentPointsProgress = PointsToNextLevel;
            OnIncreaseLevel?.Invoke();
        }
    }

    private void DecreasePoints()
    {
        if (CurrentPointsProgress < _pointsDecreasingRatePerStep)
        {
            CurrentPointsProgress = 0;
            OnDecreaseLevel?.Invoke();
            return;
        }
        
        CurrentPointsProgress -= _pointsDecreasingRatePerStep;
    }
}