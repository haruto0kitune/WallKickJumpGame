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
    [SerializeField]
    Text text;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        scoreManager = ScoreManager.Instance;

        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = "スコア登録";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Register";
        }
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
            scoreRanking["score"] = ScoreManager.score.Value;
            scoreRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }
        else
        {
            scoreRanking["name"] = "No Name";
            scoreRanking["score"] = ScoreManager.score.Value;
            scoreRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }

        if(inputField.text != "")
        {
            meterRanking["name"] = inputField.text;
            meterRanking["meter"] = ScoreManager.meter.Value;
            meterRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }
        else
        {
            meterRanking["name"] = "No Name";
            meterRanking["meter"] = ScoreManager.meter.Value;
            meterRanking["uuid"] = PlayerPrefs.GetString("uuid");
        }

        scoreRanking.Save();
        meterRanking.Save();

        ScoreManager.Instance.Reset();
        SceneManager.LoadScene("ranking");
        SceneManager.UnloadScene("result");
    }
}
