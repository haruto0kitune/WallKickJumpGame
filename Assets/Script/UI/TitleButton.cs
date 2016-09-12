using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TitleButton : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    Text text;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    void Start()
    {
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = "タイトルへ戻る";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Title";
        }
    }

    public void Title()
    {
        audioSource.PlayOneShot(audioSource.clip);
        Invoke("_Title", 0.15f);
    }

    public void _Title()
    {
        if(ScoreManager.Instance != null)
        {
            ScoreManager.Instance.Reset();
        }

        SceneManager.LoadScene("title");
        SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
    }
}
