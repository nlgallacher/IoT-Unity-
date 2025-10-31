using UnityEngine;

public class RotateAtFrequency : MonoBehaviour
{
    [Tooltip("Rotation frequency in revolutions per second.")]
    public float frequency = 1.0f; // 1 revolution per second

    [Tooltip("Rotation axis, e.g., (0,1,0) for Y axis.")]
    public Vector3 rotationAxis = Vector3.up;

    void Update()
    {
        // 360 degrees per revolution * frequency = degrees per second
        float degreesPerSecond = 360f * frequency;
        transform.Rotate(rotationAxis.normalized, degreesPerSecond * Time.deltaTime);
    }
}
