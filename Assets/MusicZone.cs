using UnityEngine;

public class MusicZone : MonoBehaviour
{
    private AudioSource audioSource;
    private static AudioSource currentlyPlaying; // Статическая переменная для отслеживания текущей музыки


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Убедитесь, что у игрока есть тег "Player"
        {
            // Если уже играет другая музыка, останавливаем её
            if (currentlyPlaying != null && currentlyPlaying != audioSource)
            {
                currentlyPlaying.Stop();
            }

            // Включаем новую музыку и запоминаем её
            audioSource.Play();
            currentlyPlaying = audioSource;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Можно добавить остановку музыки при выходе, если нужно
            audioSource.Stop();
            currentlyPlaying = null;
        }
    }
}