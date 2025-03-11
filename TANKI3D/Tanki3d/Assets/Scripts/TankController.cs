using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class TankController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 150f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public int maxBullets = 5;
    public TextMeshProUGUI bulletCountText;
    public GameObject explosionPrefab;

    public AudioSource audioSource;
    public AudioClip shootSound;
    public AudioMixer gameAudioMixer;

    private Rigidbody rb;
    private int currentBullets;
    private bool isReloading = false;
    private bool canShoot = true;
    private float fireCooldown = 0.05f;

    // Localization properties
    private string tableReference = "UI_TEXT";  // Reference to your localization table

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentBullets = maxBullets;
        UpdateBulletCountUI();
        StartCoroutine(Reload());
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
            move = Input.GetAxis("Vertical1") * moveSpeed;
            rotate = Input.GetAxis("Horizontal1") * rotateSpeed;
        }
        else if (gameObject.name == "Tank2")
        {
            move = Input.GetAxis("Vertical2") * moveSpeed;
            rotate = Input.GetAxis("Horizontal2") * rotateSpeed;
        }

        MoveTank(move, rotate);
    }

    void MoveTank(float move, float rotate)
    {
        if (move != 0)
        {
            Vector3 movement = transform.forward * move * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }

        if (rotate != 0)
        {
            Quaternion turnRotation = Quaternion.Euler(0f, rotate * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnRotation);
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

    IEnumerator Reload()
    {
        while (true)
        {
            if (currentBullets < maxBullets && !isReloading)
            {
                isReloading = true;
                yield return new WaitForSeconds(6f);
                currentBullets++;
                UpdateBulletCountUI();
                isReloading = false;
            }
            yield return null;
        }
    }

    void Shoot()
    {
        if (currentBullets > 0 && canShoot)
        {
            canShoot = false;
            StartCoroutine(ShootCooldown());

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rbBullet = bullet.GetComponent<Rigidbody>();
            rbBullet.velocity = firePoint.forward * bulletSpeed;

            if (audioSource != null && shootSound != null)
            {
                audioSource.clip = shootSound;
                audioSource.outputAudioMixerGroup = gameAudioMixer.FindMatchingGroups("ShootGroup")[0];
                audioSource.Play();
            }

            Destroy(bullet, 10f);
            currentBullets--;
            UpdateBulletCountUI();
        }
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(fireCooldown);
        canShoot = true;
    }

    public void DestroyTank()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject, 5f);
    }

    void UpdateBulletCountUI()
    {
        if (bulletCountText != null)
        {
            string localizedText = GetLocalizedString("BULLETS_COUNT");  // Get localized string
            bulletCountText.text = string.Format(localizedText, currentBullets);  // Format with current bullets count
        }
    }

    string GetLocalizedString(string key)
    {
        return LocalizationSettings.StringDatabase.GetLocalizedString(tableReference, key);
    }

    void ResetInertia()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
