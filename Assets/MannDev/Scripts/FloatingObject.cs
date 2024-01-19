using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    public float minFloatSpeed = 1.0f; // Minimum speed of floating
    public float maxFloatSpeed = 2.0f; // Maximum speed of floating
    public float minFloatHeight = 1.0f; // Minimum height of floating
    public float maxFloatHeight = 2.0f; // Maximum height of floating

    private Vector3 startPos;
    private float floatSpeed;
    private float floatHeight;

    public void Initialize()
    {
        startPos = transform.position;

        // Randomize the floating speed and height within the specified ranges.
        floatSpeed = Random.Range(minFloatSpeed, maxFloatSpeed);
        floatHeight = Random.Range(minFloatHeight, maxFloatHeight);
    }

    public void UpdateFloating()
    {
        // Calculate a new Y position based on a sine wave.
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        // Update the GameObject's position.
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    
}
