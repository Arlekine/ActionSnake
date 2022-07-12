using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StyleRank
{
    public event Action<StyleLevel> OnLevelChanged;
    
    [Range(0f, 1f)] [SerializeField] private float _rankLevelStartPointsPercent;
    [SerializeField] private StyleLevel[] _styleLevels;
    
    private int _currentStyleLevelIndex = 0;
    private List<IExternalRankMultiplier> _externalRankMultipliers = new List<IExternalRankMultiplier>();
    
    public StyleLevel CurrentStyleLevel => _styleLevels[_currentStyleLevelIndex];

    //TODO: maybe better use constructor
    public void Init()
    {
        _styleLevels[_currentStyleLevelIndex].Activate(_rankLevelStartPointsPercent);
        _styleLevels[_currentStyleLevelIndex].OnIncreaseLevel += IncreaseStyleLevel;
        _styleLevels[_currentStyleLevelIndex].OnDecreaseLevel += DecreaseStyle;
    }

    public uint GetRankedPoints(uint rawPoints, bool increaseRank = true)
    {
        if (increaseRank)
            IncreaseRank(rawPoints);

        uint summarizedExternalMultiplier = 1;
        foreach (var externalRankMultiplier in _externalRankMultipliers)
        {
            summarizedExternalMultiplier *= externalRankMultiplier.Multiplier;
        }
        
        return rawPoints * _styleLevels[_currentStyleLevelIndex].Multiplier * summarizedExternalMultiplier;
    }

    public void AddExternalMultiplier(IExternalRankMultiplier externalMultiplier)
    {
        _externalRankMultipliers.Add(externalMultiplier);
    }
    
    public void RemoveExternalMultiplier(IExternalRankMultiplier externalMultiplier)
    {
        _externalRankMultipliers.Remove(externalMultiplier);
    }

    private void IncreaseRank(uint stylePoints)
    {
        _styleLevels[_currentStyleLevelIndex].IncreasePoints(stylePoints);
    }

    private void IncreaseStyleLevel()
    {
        if (_currentStyleLevelIndex >= _styleLevels.Length - 1)
            return;

        var oldIndex = _currentStyleLevelIndex;
        _currentStyleLevelIndex++;

        SwitchStyleLevel(oldIndex, _currentStyleLevelIndex);
    }

    private void DecreaseStyle()
    {
        if (_currentStyleLevelIndex <= 0)
            return;

        var oldIndex = _currentStyleLevelIndex;
        _currentStyleLevelIndex--;

        SwitchStyleLevel(oldIndex, _currentStyleLevelIndex);
    }

    private void SwitchStyleLevel(int oldIndex, int newIndex)
    {
        _styleLevels[oldIndex].Deactivate();
        _styleLevels[oldIndex].OnIncreaseLevel -= IncreaseStyleLevel;
        _styleLevels[oldIndex].OnDecreaseLevel -= DecreaseStyle;
        
        _styleLevels[newIndex].Activate(_rankLevelStartPointsPercent);
        _styleLevels[newIndex].OnIncreaseLevel += IncreaseStyleLevel;
        _styleLevels[newIndex].OnDecreaseLevel += DecreaseStyle;
        
        OnLevelChanged?.Invoke(_styleLevels[newIndex]);
    }
}