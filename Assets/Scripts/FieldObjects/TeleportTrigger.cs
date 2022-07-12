using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    private Cell _teleportationCell;

    public void SetTeleportationCell(Cell cell)
    {
        _teleportationCell = cell;
    }

    private void OnTriggerEnter(Collider other)
    {
        var mover = other.GetComponent<SnakeHead>();

        if (mover != null)
        {
            TeleportToPoint(mover);
        }
    }

    private void TeleportToPoint(SnakeHead mover)
    {
        mover.TeleportToCell(_teleportationCell);
    }
}
