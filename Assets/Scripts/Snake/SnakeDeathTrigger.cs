using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDeathTrigger : MonoBehaviour, ISnakeHazard
{
    private void OnTriggerEnter(Collider other)
    {
        var destructable = other.GetComponent<IDestructible>();
        if (destructable != null)
        {
            destructable.Die();
        }
    }
}
