using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Restart()
    {
        audioSource.PlayOneShot(audioSource.clip);
        SceneManager.LoadScene(0);
    }
}
