using UnityEngine;

public class MoveUpDown : MonoBehaviour
{
    public float amplitude = 1.0f;  // Distance to move up and down
    public float frequency = 1.0f;  // Speed of the movement

    private Vector3 startPosition;

    void Start()
    {
        // Store the initial position of the object
        startPosition = transform.position;
    }

    void Update()
    {
        // Calculate new position based on a sine wave
        float newY = startPosition.y + amplitude * Mathf.Sin(Time.time * frequency);
        
        // Set the object's position to the new calculated position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}