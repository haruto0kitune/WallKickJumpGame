using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TitleButton : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
    }

    public void Title()
    {
        audioSource.PlayOneShot(audioSource.clip);
        Invoke("_Title", 0.15f);
    }

    public void _Title()
    {
        SceneManager.LoadScene("title");
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
    }
}
