using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource; // Додайте AudioSource через Інспектор

    public void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
