using System;

public class SnakeStepTimer
{
    public event Action OnTimer;

    public bool TimerStarted => _timerStarted;

    private int _stepsToInvoke;
    private int _currentStepCount;
    private bool _loop;
    private bool _timerStarted;

    public SnakeStepTimer(int stepsToInvoke)
    {
        _stepsToInvoke = stepsToInvoke;
    }

    public void StartTimer(bool isLoop)
    {
        if (_timerStarted)
            throw new Exception($"Timer already started");
        
        _timerStarted = true;
        _currentStepCount = 0;
        _loop = isLoop;
        SnakeStepTime.OnStepComplete += Step;
    }

    public void StopTimer()
    {
        if (_timerStarted == false)
            throw new Exception($"Timer is not started");

        _timerStarted = false;
        SnakeStepTime.OnStepComplete -= Step;
    }

    private void Step()
    {
        _currentStepCount++;
        if (_currentStepCount >= _stepsToInvoke)
        {
            OnTimer?.Invoke();

            if (_loop)
                _currentStepCount = 0;
            else
                SnakeStepTime.OnStepComplete -= Step;
        }
    }
}