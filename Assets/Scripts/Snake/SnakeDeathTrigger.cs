using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDeathTrigger : MonoBehaviour, ISnakeHazard
{
    private void OnTriggerEnter(Collider other)
    {
        var snake = other.GetComponent<SnakeHead>();
        if (snake != null)
        {
            snake.Die();
        }
    }
}
