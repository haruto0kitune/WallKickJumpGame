using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Block : MonoBehaviour, IPause
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        PauseManager.pausers.Add(this);

        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ =>
            {
                PauseManager.pausers.Remove(this);
                Destroy(this.gameObject);
            });
    }

    public void Pause()
    {
        animator.speed = 0;
    }

    public void Resume()
    {
        animator.speed = 1;
    }

    void OnDestroy()
    {
        PauseManager.pausers.Remove(this);
    }
}
