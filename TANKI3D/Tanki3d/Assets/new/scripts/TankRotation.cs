using UnityEngine;

public class TankRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Швидкість обертання
    public float maxRotationAngle = 45.0f; // Максимальний кут обертання (в градусах)

    private float currentAngle = 0.0f;

    void Update()
    {
        // Обчислюємо новий кут обертання за допомогою синусоїди
        currentAngle = Mathf.Sin(Time.time * rotationSpeed) * maxRotationAngle;

        // Застосовуємо обертання до об'єкта
        transform.rotation = Quaternion.Euler(0, currentAngle, 0); // Обертання по осі Y
    }
}
