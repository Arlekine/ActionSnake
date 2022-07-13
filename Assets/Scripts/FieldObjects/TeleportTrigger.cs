using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : FieldObject
{
    private CellCoordinates _teleportationCell;

    public void SetTeleportationCell(CellCoordinates cell)
    {
        _teleportationCell = cell;
    }

    protected override void InteractWithSnake(SnakeHead snake)
    {
        TeleportToPoint(snake);
    }

    private void TeleportToPoint(SnakeHead mover)
    {
        mover.TeleportToCell(_teleportationCell);
    }
}
