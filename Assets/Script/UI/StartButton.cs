using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartButton : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public void GameStart()
    {
        audioSource.PlayOneShot(audioSource.clip);
        SceneManager.LoadScene(0);
        SceneManager.UnloadScene(2);
    }
}
