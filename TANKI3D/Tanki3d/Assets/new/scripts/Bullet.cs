using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5.5f; // Постійна швидкість кулі
    public GameObject explosionPrefab; // Префаб вибуху

    public AudioClip ricochetSound; // Звук рикошету
    public AudioClip explosionSound; // Звук вибуху

    public AudioMixerGroup explosionMixerGroup; // Група мікшування для вибуху
    public AudioMixerGroup ricochetMixerGroup; // Група мікшування для рикошету

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed; // Початкова швидкість кулі
    }

    void FixedUpdate()
    {
        // Збереження сталої швидкості в фізичному оновленні
        if (rb.velocity.magnitude != bulletSpeed)
        {
            rb.velocity = rb.velocity.normalized * bulletSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Якщо куля влучає в танк
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Танк знищено!");

            // Генерація вибуху, якщо є префаб
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            }

            // Відтворення звуку вибуху
            PlaySound(explosionSound, explosionMixerGroup, collision.transform.position);

            Destroy(collision.gameObject); // Знищити танк
            Destroy(gameObject); // Знищити кулю
        }
        // Рикошет
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Рикошет!");
            PlaySound(ricochetSound, ricochetMixerGroup, transform.position); // Відтворення звуку рикошету
        }
    }

    private void PlaySound(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position)
    {
        if (clip == null) return;

        // Створення нового об'єкта для відтворення звуку
        GameObject soundObject = new GameObject("SoundObject");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixerGroup; // Встановлення групи мікшування
        audioSource.spatialBlend = 1.0f; // Просторовий звук
        audioSource.Play();

        // Видалення об'єкта після завершення звуку
        Destroy(soundObject, clip.length);
    }

    public void SetVolume(float volume)
    {
        // Зміна гучності для всіх звукових груп
        explosionMixerGroup.audioMixer.SetFloat("ExplosionVolume", volume);
        ricochetMixerGroup.audioMixer.SetFloat("RicochetVolume", volume);
    }
}