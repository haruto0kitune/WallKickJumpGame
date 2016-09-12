using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class PauseButton : MonoBehaviour
{
    [SerializeField]
    GameObject pauseManager;
    PauseManager pauseManagerComponent;
    AudioSource audioSource;

    void Awake()
    {
        pauseManagerComponent = pauseManager.GetComponent<PauseManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        //this.ObserveEveryValueChanged(x => isMerge)
        //    .Where(x => x)
        //    .Do(x => isMerge = false)
        //    .DelayFrame(10)
        //    .Subscribe(_ => SceneManager.MergeScenes(SceneManager.GetSceneByName("pause"), SceneManager.GetSceneByName("test")));
    }

    public void Pause()
    {
        audioSource.PlayOneShot(audioSource.clip);
        pauseManagerComponent.Pause();
    }
}
