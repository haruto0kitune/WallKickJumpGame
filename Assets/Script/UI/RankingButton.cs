using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RankingButton : MonoBehaviour
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
            text.text = "ランキング";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Ranking";
        }
    }

    public void Ranking()
    {
        audioSource.PlayOneShot(audioSource.clip);
        Invoke("_Ranking", 0.1f);
    }

    void _Ranking()
    {
        SceneManager.LoadScene("ranking");
        SceneManager.UnloadScene("result");
    }
}
