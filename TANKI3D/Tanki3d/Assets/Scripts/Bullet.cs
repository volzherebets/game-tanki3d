using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 5.5f;
    public GameObject explosionPrefab;
    public AudioClip ricochetSound;
    public AudioClip explosionSound;
    public AudioMixerGroup explosionMixerGroup;
    public AudioMixerGroup ricochetMixerGroup;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * bulletSpeed;
    }

    void FixedUpdate()
    {
        if (rb.velocity.magnitude != bulletSpeed)
        {
            rb.velocity = rb.velocity.normalized * bulletSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Танк знищено!");
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            }
            PlaySound(explosionSound, explosionMixerGroup, collision.transform.position);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Рикошет!");
            PlaySound(ricochetSound, ricochetMixerGroup, transform.position);
        }
    }

    private void PlaySound(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position)
    {
        if (clip == null) return;
        GameObject soundObject = new GameObject("SoundObject");
        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.spatialBlend = 1.0f;
        audioSource.Play();
        Destroy(soundObject, clip.length);
    }

    public void SetVolume(float volume)
    {
        explosionMixerGroup.audioMixer.SetFloat("ExplosionVolume", volume);
        ricochetMixerGroup.audioMixer.SetFloat("RicochetVolume", volume);
    }
}