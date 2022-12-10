using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    // The NPC's starting position
    Vector3 startPosition;

    // The maximum height the NPC should reach
    public float maxHeight = 2.0f;

    // The speed at which the NPC should move
    public float speed = 1.0f;

    // The direction the NPC is currently moving (1 = up, -1 = down)
    int direction = 1;

    void Start()
    {
        // Save the NPC's starting position
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate the NPC's new position
        Vector3 newPosition = transform.position + Vector3.up * direction * speed * Time.deltaTime;

        // If the NPC has reached the maximum height, change direction
        if (newPosition.y > startPosition.y + maxHeight || newPosition.y < startPosition.y)
        {
            direction *= -1;
        }

        // Update the NPC's position
        transform.position = newPosition;
    }
}
