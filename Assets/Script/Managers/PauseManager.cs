using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    GameObject root;
    [SerializeField]
    GameObject pauseUI;

    public static List<IPause> pausers;
    public static bool isPausing { get; private set; }

    void Awake()
    {
        pausers = new List<IPause>();
    }

    void Start()
    {
        this.ObserveEveryValueChanged(x => pauseUI.activeSelf)
            .Pairwise()
            .Where(x => x.Previous && !x.Current)
            .DelayFrame(10)
            .Subscribe(_ => isPausing = false);
    }

    public void Pause()
    {
        isPausing = true;
        pausers.RemoveAll(x => x == null);
        pauseUI.SetActive(true);

        foreach (var item in pausers)
        {
            if (item != null)
            {
                item.Pause();
            }
        }
    }

    public void Resume()
    {
        foreach (var item in pausers)
        {
            item.Resume();
        }
    }
}
