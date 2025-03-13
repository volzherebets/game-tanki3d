using UnityEngine;

public class TankRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f; 
    public float maxRotationAngle = 45.0f; 

    private float currentAngle = 0.0f;

    void Update()
    {
        currentAngle = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;

        transform.rotation = Quaternion.Euler(0, currentAngle, 0); 
    }
}