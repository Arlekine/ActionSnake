using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class FieldObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var snake = other.GetComponent<SnakeHead>();
        if (snake != null)
        {
            InteractWithSnake(snake);
        }
    }

    protected abstract void InteractWithSnake(SnakeHead snake);
}