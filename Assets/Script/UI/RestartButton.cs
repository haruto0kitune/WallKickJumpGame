using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    GameObject restartManager;
    RestartManager restartManagerComponent;
    [SerializeField]
    GameObject pauseUI;
    AudioSource audioSource;
    [SerializeField]
    Text text;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = "リトライ";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Restart";
        }

        if (SceneManager.GetActiveScene().name == "test")
        {
            restartManagerComponent = restartManager.GetComponent<RestartManager>();
        }
    }

    public void Restart()
    {
        audioSource.PlayOneShot(audioSource.clip);
        _Restart();
    }

    void _Restart()
    {
        ScoreManager.Instance.Reset();

        if (SceneManager.GetActiveScene().name == "test")
        {
            StartCoroutine(restartManagerComponent.Restart());
        }

        if (SceneManager.GetActiveScene().name == "result")
        {
            SceneManager.LoadScene("test");
        }
    }
}
