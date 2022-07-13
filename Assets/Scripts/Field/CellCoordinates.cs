using System;

public struct CellCoordinates
{
    public readonly int X;
    public readonly int Y;

    public CellCoordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static bool operator ==(CellCoordinates a, CellCoordinates b)
    {
        bool isDirectionsTheSame = a.X == b.X && a.Y == b.Y;

        return isDirectionsTheSame;
    }
    
    public static bool operator !=(CellCoordinates a, CellCoordinates b)
    {
        return a.X != b.X || a.Y != b.Y;
    }
    
    public static CellCoordinates operator + (CellCoordinates cell, MoveDirection direction)
    {
        return new CellCoordinates(cell.X + direction.X, cell.Y + direction.Y);
    }

    public override bool Equals(object obj)
    {
        if (obj is CellCoordinates coordinates)
        {
            return this == coordinates;
        }

        return false;
    }
}