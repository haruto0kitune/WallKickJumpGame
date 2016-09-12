using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ResumeButton : MonoBehaviour 
{
    [SerializeField]
    GameObject pauseManager;
    PauseManager pauseManagerComponent;
    [SerializeField]
    GameObject pauseUI;
    AudioSource audioSource;
    [SerializeField]
    Text text;

    void Awake ()
    {
        if(Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = "ゲーム再開";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Resume";
        }

        pauseManagerComponent = pauseManager.GetComponent<PauseManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start () 
    {
        this.ObserveEveryValueChanged(x => pauseManagerComponent)
            .Where(x => pauseManagerComponent == null)
            .Subscribe(_ => Debug.Log("pauseManagerComponent is null"));
    }

    public void Resume()
    {
        audioSource.PlayOneShot(audioSource.clip);
        Invoke("_Resume", 0.15f);
        pauseUI.SetActive(false);
    }

    void _Resume()
    {
        pauseManagerComponent.Resume();
    }
}
