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
    }

    public void Pause()
    {
        if (!pauseManagerComponent.isPausing)
        {
            audioSource.PlayOneShot(audioSource.clip);
            pauseManagerComponent.Pause();
        }
    }
}
