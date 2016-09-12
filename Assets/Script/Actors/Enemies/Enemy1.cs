using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Enemy1 : MonoBehaviour, IPause, IReset
{
    Rigidbody2D _rigidbody2d;
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField]
    float startPoint;
    [SerializeField]
    float endPoint;
    [SerializeField]
    int maxTime;

    IEnumerator enumeratorStore;
    Coroutine coroutineStore;

    void OnEnable()
    {
        //Debug.Log("enable");
    }

    void Awake()
    {
        //Debug.Log("awake");
        enumeratorStore = Move();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        PauseManager.pausers.Add(this);
        ResetManager.resetComponents.Add(this);

        //this.ObserveEveryValueChanged(x => coroutineStore)
        //    .Where(x => x == null)
        //    .Subscribe(_ => Debug.Log("coroutineStore null"));

        this.FixedUpdateAsObservable()
            .Where(x => coroutineStore == null)
            .Subscribe(_ => coroutineStore = StartCoroutine(enumeratorStore));

        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ =>
            {
                PauseManager.pausers.Remove(this);
                Destroy(this.gameObject);
            });
    }

    void Turn()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    IEnumerator Move()
    {
        while (true)
        {
            if (gameObject == null) break;

            foreach (var x in Utility.Ease(startPoint, endPoint, 0, maxTime))
            {
                transform.position = new Vector2(x, transform.position.y);
                yield return new WaitForFixedUpdate();
            }

            Turn();

            foreach (var x in Utility.Ease(-startPoint, -endPoint, 0, maxTime))
            {
                transform.position = new Vector2(x, transform.position.y);
                yield return new WaitForFixedUpdate();
            }

            Turn();
        }
    }
     
    public void Pause()
    {
        StopCoroutine(coroutineStore);

        if (animator != null)
        {
            animator.speed = 0;
        }
        else
        {
            Debug.Log("animator is null");
        }
    }

    public void Resume()
    {
        coroutineStore = StartCoroutine(enumeratorStore);
        animator.speed = 1;
    }

    public void Reset()
    {
        enumeratorStore = Move();
        //Debug.Log("Reset value: " + enumeratorStore);
        _rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void OnDestroy()
    {
        PauseManager.pausers.Remove(this);
    }
}
