using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StylePointsView : MonoBehaviour
{
    private const string PointsAddingFormat = "+{0}";
    private const string PointsFormat = "{0:00000000} pts";
    private const string StyleFormat = "x {0}";
    
    [SerializeField] private StylePointsCounter _pointsCounter;

    [Space] 
    [SerializeField] private TextMeshProUGUI _points;
    [SerializeField] private TextMeshProUGUI _pointsAddingText;
    [SerializeField] private TextMeshProUGUI _styleRank;

    [Space]
    [SerializeField] private int _pointsAddingHideTime = 3;

    private SnakeStepTimer _adddingTextHideTimer;
    
    private void Start()
    {
        _adddingTextHideTimer = new SnakeStepTimer(_pointsAddingHideTime);
        _adddingTextHideTimer.OnTimer += ClearAddingText;

        ClearAddingText();
        _points.text = String.Format(PointsFormat, 0);
        _styleRank.text = String.Format(StyleFormat, _pointsCounter.StyleRank.CurrentStyleLevel.Multiplier);
        
        _pointsCounter.OnPointsIncreased += UpdateScore;
        _pointsCounter.StyleRank.OnLevelChanged += UpdateStyleLevel;
    }

    private void UpdateScore(uint addingValue)
    {
        _pointsAddingText.text = String.Format(PointsAddingFormat, addingValue);
        _points.text = String.Format(PointsFormat, _pointsCounter.CurrentPoints);
        
        if (_adddingTextHideTimer.TimerStarted)
            _adddingTextHideTimer.StopTimer();
        
        _adddingTextHideTimer.StartTimer(false);
    }

    private void UpdateStyleLevel(StyleLevel newStyleLevel)
    {
        _styleRank.text = String.Format(StyleFormat, newStyleLevel.Multiplier);
    }

    private void ClearAddingText()
    {
        _pointsAddingText.text = "";
    }
}
