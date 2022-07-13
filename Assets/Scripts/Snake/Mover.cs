using System;
using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public event Action OnHalfWay;
    public event Action OnMoveComplete;

    private Coroutine _currentMoveRoutine;

    public void Move(Vector3 startPoint, Vector3 endPoint, float time)
    {
        if (_currentMoveRoutine != null)
            StopCoroutine(_currentMoveRoutine);
        
        _currentMoveRoutine = StartCoroutine(MovingRoutine(startPoint, endPoint, time));
    }

    private IEnumerator MovingRoutine(Vector3 startPoint, Vector3 endPoint, float time)
    {
        var currentTime = 0f;
        var isHalfWayTriggered = false;

        while (currentTime <= time)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, currentTime / time);

            yield return null;
            currentTime += Time.deltaTime;

            if (isHalfWayTriggered == false && currentTime >= time * 0.5f)
            {
                isHalfWayTriggered = true;
                OnHalfWay?.Invoke();
            }
        }
        
        OnMoveComplete?.Invoke();
    }
}