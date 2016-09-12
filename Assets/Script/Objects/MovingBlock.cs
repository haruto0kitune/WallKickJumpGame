using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class MovingBlock : MonoBehaviour, IPause, IReset
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

    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enumeratorStore = Move();
    }

    void Start()
    {
        PauseManager.pausers.Add(this);
        ResetManager.resetComponents.Add(this);

        //this.ObserveEveryValueChanged(x => coroutineStore)
        //    .Where(x => x == null)
        //    .Subscribe(_ => Debug.Log("coroutineStore null"));

        //this.ObserveEveryValueChanged(x => coroutineStore)
        //    .Where(x => x == null)
        //    .Subscribe(_ => Debug.Log("coroutineStore null"));

        this.FixedUpdateAsObservable()
            .Take(1)
            .Subscribe(_ => coroutineStore = StartCoroutine(enumeratorStore));

        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ =>
            {
                PauseManager.pausers.Remove(this);
                Destroy(this.gameObject);
            });

        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ => _.gameObject.transform.parent = transform);

        this.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Do(x => Debug.Log("ExitMovingBlock"))
            .Where(x => !x.gameObject.GetComponent<PlayerState>().isSticking.Value && !x.gameObject.GetComponent<PlayerState>().isGrounded.Value)
            .Subscribe(_ => _.gameObject.transform.parent = GameObject.Find("Actors").transform);
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
        animator.speed = 0;
    }

    public void Resume()
    {
        coroutineStore = StartCoroutine(enumeratorStore);
        animator.speed = 1;
    }

    public void Reset()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enumeratorStore = Move();
    }

    void OnDestroy()
    {
        PauseManager.pausers.Remove(this);
    }
}