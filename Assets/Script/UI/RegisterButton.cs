using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class RegisterButton : MonoBehaviour
{
    [SerializeField]
    InputField inputField;
    ScoreManager scoreManager;
    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

    public void RegisterRanking()
    {
        audioSource.PlayOneShot(audioSource.clip);
        _RegisterRanking();
    }

    void _RegisterRanking()
    {
        NCMBObject scoreRanking = new NCMBObject("scoreRanking");
        NCMBObject meterRanking = new NCMBObject("meterRanking");

        if(inputField.text != "")
        {
            scoreRanking["name"] = inputField.text;
            scoreRanking["score"] = scoreManager.score.Value;
            scoreRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }
        else
        {
            scoreRanking["name"] = "No Name";
            scoreRanking["score"] = scoreManager.score.Value;
            scoreRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }

        if(inputField.text != "")
        {
            meterRanking["name"] = inputField.text;
            meterRanking["meter"] = scoreManager.meter.Value;
            meterRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }
        else
        {
            meterRanking["name"] = "No Name";
            meterRanking["meter"] = scoreManager.meter.Value;
            meterRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }

        scoreRanking.Save();
        meterRanking.Save();

        SceneManager.LoadScene("ranking");
        SceneManager.UnloadScene("result");
    }
}
