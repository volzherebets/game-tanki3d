using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 5f; // Швидкість руху танка
    public float rotateSpeed = 150f; // Швидкість обертання танка
    public GameObject bulletPrefab; // Префаб кулі
    public Transform firePoint; // Точка виходу кулі
    public float bulletSpeed = 20f; // Швидкість польоту кулі
    public int maxBullets = 5; // Максимальна кількість патронів в обоймі
    public TextMeshProUGUI bulletCountText; // Текст для відображення кількості патронів (TextMeshPro)
    public GameObject explosionPrefab; // Префаб вибуху

    public AudioSource audioSource; // Джерело звуку для пострілу
    public AudioClip shootSound; // Звук пострілу
    public AudioMixer gameAudioMixer; // Підключаємо Audio Mixer для налаштування гучності

    private Rigidbody rb;
    private int currentBullets; // Поточна кількість патронів
    private bool isReloading = false; // Чи йде перезарядка
    private bool canShoot = true; // Прапорець для контролю кд
    private float fireCooldown = 0.05f; // Час кд між пострілами

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentBullets = maxBullets; // Заповнити обойму
        UpdateBulletCountUI();
        StartCoroutine(Reload()); // Розпочати корутіну для автоматичної перезарядки
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleShooting();
        ResetInertia();
    }

    void HandleMovement()
    {
        float move = 0;
        float rotate = 0;

        if (gameObject.name == "Tank1")
        {
            // Рух для танка 1
            move = Input.GetAxis("Vertical1") * moveSpeed;
            rotate = Input.GetAxis("Horizontal1") * rotateSpeed;
        }
        else if (gameObject.name == "Tank2")
        {
            // Рух для танка 2
            move = Input.GetAxis("Vertical2") * moveSpeed;
            rotate = Input.GetAxis("Horizontal2") * rotateSpeed;
        }

        MoveTank(move, rotate);
    }

    void MoveTank(float move, float rotate)
    {
        // Переміщення вперед/назад
        if (move != 0)
        {
            Vector3 movement = transform.forward * move * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement); // Використовуємо Rigidbody для руху
        }

        // Обертання
        if (rotate != 0)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, rotate * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation); // Обертання через Rigidbody
        }
    }

    void HandleShooting()
    {
        if (gameObject.name == "Tank1" && Input.GetKeyDown(KeyCode.Q))
        {
            Shoot();
        }
        else if (gameObject.name == "Tank2" && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    // Автоматична перезарядка
    IEnumerator Reload()
    {
        while (true)
        {
            // Чекати, поки не закінчаться патрони
            if (currentBullets < maxBullets && !isReloading)
            {
                isReloading = true;
                yield return new WaitForSeconds(6f); // Час між перезарядками
                currentBullets++; // Збільшуємо кількість патронів на 1
                UpdateBulletCountUI(); // Оновлюємо UI
                isReloading = false;
            }
            yield return null; // Чекати наступного кадру
        }
    }

    void Shoot()
    {
        if (currentBullets > 0 && canShoot) // Стрільба лише якщо є патрони та немає кд
        {
            // Встановлюємо кд
            canShoot = false;
            StartCoroutine(ShootCooldown());

            // Створення кулі
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
            rbBullet.velocity = firePoint.forward * bulletSpeed;

            // Відтворення звуку пострілу через Audio Mixer
            if (audioSource != null && shootSound != null)
            {
                audioSource.clip = shootSound;
                audioSource.outputAudioMixerGroup = gameAudioMixer.FindMatchingGroups("ShootGroup")[0]; // Прив’язуємо до групи
                audioSource.Play();
            }

            // Знищити кулю через 10 секунд
            Destroy(bullet, 10f);

            currentBullets--; // Зменшуємо кількість патронів
            UpdateBulletCountUI(); // Оновлюємо текст на UI
        }
    }

    // Корутина для оновлення кд
    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
    }

    public void DestroyTank()
    {
        // Відображення вибуху
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject, 5f); // Знищити танк
    }

    // Оновлення інтерфейсу з кількістю патронів
    void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "BULLETS: " + currentBullets; // Оновлення тексту
        }
    }

    // Очищення інерції
    void ResetInertia()
    {
        rb.velocity = Vector3.zero; // Очищаємо лінійну швидкість
        rb.angularVelocity = Vector3.zero; // Очищаємо кутову швидкість
    }
}