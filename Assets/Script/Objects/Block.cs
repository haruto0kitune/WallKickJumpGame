using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class Block : MonoBehaviour, IPause
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        foreach (var item in this.gameObject.Ancestors().Where(x => x.name == "Game").Descendants().Where(x => x.name == "PauseManager"))
        {
            item.GetComponent<PauseManager>().pausers.Add(this);
        }

        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(this.gameObject));
    }

    public void Pause()
    {
        animator.speed = 0;
    }

    public void Resume()
    {
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
