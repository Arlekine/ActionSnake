using UnityEngine;

public class StyleMoveInitializer : MonoBehaviour
{
    [Space]
    [SerializeField] private int _tightMovePointsPerHazard = 100;
    
    [Space]
    [SerializeField] private int _deadAHeadMove = 1000;
    
    [Space]
    [SerializeField] private int _uroborosMove = 300;
    [SerializeField] private int _uroborosDistance = 6;
    
    [Space]
    [SerializeField] private int _lockStatePoints = 300;
    [SerializeField] private int _lockStateRankMultiplier = 6;
    
    [Space]
    [SerializeField] private SnakeHead _snakeHead;
    [SerializeField] private StylePointsCounter _stylePointsCounter;

    private void Awake()
    {
        //var tightMove = new TightMove(_snakeHead, _stylePointsCounter, _tightMovePointsPerHazard);
        //var deadAHead = new DeadAHeadMove(_snakeHead, _stylePointsCounter, _deadAHeadMove);
        //var uroboros = new UroborosMove(_snakeHead, _stylePointsCounter, _uroborosMove, _uroborosDistance);
        var lockState = new LockStateMove(_snakeHead, _stylePointsCounter, _lockStatePoints, _lockStateRankMultiplier);
    }
}