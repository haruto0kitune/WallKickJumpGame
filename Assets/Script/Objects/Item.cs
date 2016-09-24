using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class Item : MonoBehaviour, IPause
{
    AudioSource audioSource;
    Animator animator;

    bool mustDestroy;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        foreach (var item in this.gameObject.Ancestors().Where(x => x.name == "Game").Descendants().Where(x => x.name == "PauseManager"))
        {
            item.GetComponent<PauseManager>().pausers.Add(this);
        }

        this.ObserveEveryValueChanged(x => animator)
            .Where(x => x == null)
            .Subscribe(_ => Debug.Log("animator null"));

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ =>
            {
                ScoreManager.Instance.score.Value++;
                audioSource.PlayOneShot(audioSource.clip);
                mustDestroy = true; 
            }).AddTo(this.gameObject);

        this.ObserveEveryValueChanged(x => mustDestroy)
            .Where(x => x)
            .Subscribe(_ =>
            {
                GameObject.Find("PauseManager").GetComponent<PauseManager>().pausers.Remove(this);
                Destroy(this.gameObject, 0.1f);
            });
    }

    public void Pause()
    {
        GameObject.Find("PauseManager").GetComponent<PauseManager>().pausers.RemoveAll(x => x == null);
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    public void Resume()
    {
        GameObject.Find("PauseManager").GetComponent<PauseManager>().pausers.RemoveAll(x => x == null);
        animator.speed = 1;
    }

    public void OnDestroy()
    {
        if(GameObject.Find("PauseManager") != null)
        {
            GameObject.Find("PauseManager").GetComponent<PauseManager>().pausers.Remove(this);
        }
    }
}
