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
        _Restart();
    }

    void _Restart()
    {
        ScoreManager.Instance.Reset();
        SceneManager.LoadScene("test");
        SceneManager.UnloadScene("result");
    }
}
