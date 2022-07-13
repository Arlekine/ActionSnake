using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public event Action OnHalfWay;
    public event Action OnMoveComplete;

    private Coroutine _currentMoveRoutine;

    private Vector3 _startPoint;
    private Vector3 _endPoint;
    private float _currentTime;
    private float _targetTime;
    private bool _isHalfWayTriggered;

    private void Awake()
    {
        enabled = false;
    }

    public void Move(Vector3 startPoint, Vector3 endPoint, float time)
    {
        _isHalfWayTriggered = false;

        _currentTime = 0f;
        _startPoint = startPoint;
        _endPoint = endPoint;
        _targetTime = time;

        enabled = true;
    }

    private void Update()
    {
        MoveStep(_startPoint, _endPoint, _targetTime);
    }

    private void MoveStep(Vector3 startPoint, Vector3 endPoint, float time)
    {
        transform.position = Vector3.Lerp(startPoint, endPoint, _currentTime / time);

        _currentTime += Time.deltaTime;

        if (_isHalfWayTriggered == false && _currentTime >= time * 0.5f)
        {
            _isHalfWayTriggered = true;
            OnHalfWay?.Invoke();
        }

        if (_currentTime > time)
        {
            enabled = false;
            OnMoveComplete?.Invoke();
        }
    }
}