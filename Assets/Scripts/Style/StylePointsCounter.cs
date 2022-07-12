using System;
using UnityEngine;

//TODO: may be it shouldn't be MonoBeh
public class StylePointsCounter : MonoBehaviour
{
    public event Action<uint> OnPointsIncreased; 
    
    [SerializeField] private StyleRank _styleRank;

    private uint _currentPoints;
    private uint _record;

    public uint CurrentPoints => _currentPoints;
    public uint Record => _record;
    public StyleRank StyleRank => _styleRank;

    private void Awake()
    {
        _styleRank.Init();
    }

    public void IncreasePoints(uint rawPoints, bool increaseRank = true)
    {
        if (rawPoints == 0)
            throw new ArgumentException($"Raw points can be only positive");

        _currentPoints += _styleRank.GetRankedPoints(rawPoints, increaseRank);

        if (_currentPoints > _record)
            _record = _currentPoints;
        
        OnPointsIncreased?.Invoke(rawPoints);
    }
}