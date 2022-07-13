using System;
using System.Collections.Generic;
using UnityEngine;

public struct MoveDirection
{
    public readonly int X;
    public readonly int Y;

    private MoveDirection(int x, int y)
    {
        X = x;
        Y = y;
    }

    public static MoveDirection Right = new MoveDirection(1, 0);
    public static MoveDirection Left = new MoveDirection(-1, 0);
    public static MoveDirection Up = new MoveDirection(0, 1);
    public static MoveDirection Down = new MoveDirection(0, -1);

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
        return new MoveDirection(-direction.X, -direction.Y);
    }

    public static MoveDirection[] GetSideDirections(MoveDirection forwardDirection)
    {
        MoveDirection[] sideDirections;
        
        var isForwardVertical = Math.Abs(forwardDirection.X) == 0;
        if (isForwardVertical)
        {
            var isForwardUp = forwardDirection.Y > 0;
            if (isForwardUp)
                sideDirections = new []{Left, Right};
            else
                sideDirections = new []{Right, Left};
        }
        else
        {
            var isForwardRight = forwardDirection.X > 0;
            if (isForwardRight)
                sideDirections = new []{Up, Down};
            else
                sideDirections = new []{Down, Up};
        }

        return sideDirections;
    }

    public static bool operator ==(MoveDirection directionA, MoveDirection directionB)
    {
        return directionA.X == directionB.X && directionA.Y == directionB.Y;
    }
    
    public static bool operator !=(MoveDirection directionA, MoveDirection directionB)
    {
        return directionA.X != directionB.X || directionA.Y != directionB.Y;
    }

    public override bool Equals(object obj)
    {
        if (obj is MoveDirection direction)
        {
            return this == direction;
        }

        return false;
    }
}