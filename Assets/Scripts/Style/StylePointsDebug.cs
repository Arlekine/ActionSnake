using UnityEngine;

public class StylePointsDebug : MonoBehaviour
{
    [SerializeField] private StylePointsCounter _stylePointsCounter;

    private SnakeStepTimer _timer;
    
    private void Awake()
    {
        var timer = new SnakeStepTimer(5);
        timer.OnTimer += SetPoints;
        timer.StartTimer(true);
        _timer = timer;
    }

    private void SetPoints()
    {
        _stylePointsCounter.IncreasePoints(100);
        print(_stylePointsCounter.CurrentPoints + " - x" + _stylePointsCounter.StyleRank.CurrentStyleLevel.Multiplier + " - " + _stylePointsCounter.StyleRank.CurrentStyleLevel.CurrentPointsProgress);
    }

    [EditorButton]
    private void StopTimer()
    {
        _timer.StopTimer();
        SnakeStepTime.OnStepComplete += () => { print(_stylePointsCounter.CurrentPoints + " - x" + _stylePointsCounter.StyleRank.CurrentStyleLevel.Multiplier + " - " + _stylePointsCounter.StyleRank.CurrentStyleLevel.CurrentPointsProgress); };
    }
}