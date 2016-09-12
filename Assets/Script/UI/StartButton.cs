using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class StartButton : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField]
    Text text;
    //[SerializeField]
    //Text uuid;

    void Start()
    {
        if(Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = "ゲーム スタート";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Game Start";
        }

        audioSource = GetComponent<AudioSource>();

        if(PlayerPrefs.GetString("uuid") == "")
        {
            System.Guid guid = System.Guid.NewGuid();
            var _guid = guid.ToString();

            PlayerPrefs.SetString("uuid", _guid);
            PlayerPrefs.Save();

            NCMBObject scoreRanking = new NCMBObject("scoreRanking");
            NCMBObject meterRanking = new NCMBObject("meterRanking");

            scoreRanking["uuid"] = _guid;
            meterRanking["uuid"] = _guid;

            scoreRanking.SaveAsync();
            meterRanking.SaveAsync();
        }

        //this.UpdateAsObservable().Select(x => PlayerPrefs.GetString("uuid")).SubscribeToText(uuid);
    }

    public void GameStart()
    {
        audioSource.PlayOneShot(audioSource.clip);
        _GameStart();
    }

    void _GameStart()
    {
        SceneManager.LoadScene("test");
        SceneManager.UnloadScene("title");
    }
}
