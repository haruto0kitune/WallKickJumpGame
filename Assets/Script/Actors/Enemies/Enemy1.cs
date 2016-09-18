using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using System.Linq;

public class Enemy1 : MonoBehaviour, IPause
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField]
    float startPoint;
    [SerializeField]
    float endPoint;
    [SerializeField]
    int maxTime;
    int debugCount;

    IEnumerator enumeratorStore;
    Coroutine coroutineStore;

    void Awake()
    {
        enumeratorStore = Move();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {

        foreach (var item in this.gameObject.Ancestors().Where(x => x.name == "Game").Descendants().Where(x => x.name == "PauseManager"))
        {
            item.GetComponent<PauseManager>().pausers.Add(this);
        }

        this.FixedUpdateAsObservable()
            .Where(x => coroutineStore == null)
            .ThrottleFirstFrame(1)
            .Subscribe(_ => coroutineStore = StartCoroutine(enumeratorStore));

        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(this.gameObject));
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

    public void OnDestroy()
    {
        if (GameObject.Find("PauseManager") != null)
        {
            GameObject.Find("PauseManager").GetComponent<PauseManager>().pausers.Remove(this);
        }
    }
}