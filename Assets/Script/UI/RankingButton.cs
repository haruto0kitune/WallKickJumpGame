using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RankingButton : MonoBehaviour
{
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
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
