using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveDirection
{
    public Vector3 WorldDirection => _worldDirection;
    public Vector2Int FieldCoordinatesDirection => _fieldCoordinatesDirection;

    private Vector3 _worldDirection;
    private Vector2Int _fieldCoordinatesDirection;

    private MoveDirection(Vector3 worldDirection)
    {
        _worldDirection = worldDirection;
        _fieldCoordinatesDirection = new Vector2Int((int)worldDirection.x, (int)worldDirection.z);
    }

    public static MoveDirection Right = new MoveDirection(new Vector3(1f, 0f, 0f));
    public static MoveDirection Left = new MoveDirection(new Vector3(-1f, 0f, 0f));
    public static MoveDirection Up = new MoveDirection(new Vector3(0f, 0f, 1f));
    public static MoveDirection Down = new MoveDirection(new Vector3(0f, 0f, -1f));

    public static List<MoveDirection> GetAllPossibleDirections()
    {
        var result = new List<MoveDirection>();
        
        result.Add(Right);
        result.Add(Left);
        result.Add(Up);
        result.Add(Down);

        return result;
    }

    public static MoveDirection GetOppositeDirection(MoveDirection direction)
    {
        var newDirection = new MoveDirection(-direction.WorldDirection);
        return newDirection;
    }

    public static MoveDirection[] GetSideDirections(MoveDirection forwardDirection)
    {
        MoveDirection[] sideDirections;
        
        var isForwardVertical = Math.Abs(forwardDirection.WorldDirection.x) < float.Epsilon;
        if (isForwardVertical)
        {
            var isForwardUp = forwardDirection.WorldDirection.z > 0;
            if (isForwardUp)
                sideDirections = new []{Left, Right};
            else
                sideDirections = new []{Right, Left};
        }
        else
        {
            var isForwardRight = forwardDirection.WorldDirection.x > 0;
            if (isForwardRight)
                sideDirections = new []{Up, Down};
            else
                sideDirections = new []{Down, Up};
        }

        return sideDirections;
    }

    public static bool operator ==(MoveDirection directionA, MoveDirection directionB)
    {
        bool isDirectionsTheSame = directionA.FieldCoordinatesDirection.x == directionB.FieldCoordinatesDirection.x &&
                                   directionA.FieldCoordinatesDirection.y == directionB.FieldCoordinatesDirection.y;

        return isDirectionsTheSame;
    }
    
    public static bool operator !=(MoveDirection directionA, MoveDirection directionB)
    {
        return (directionA == directionB) == false;
    }
}