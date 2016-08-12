using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Enemy1 : MonoBehaviour
{
    Rigidbody2D _rigidbody2d;
    SpriteRenderer spriteRenderer;
    [SerializeField]
    float startPoint;
    [SerializeField]
    float endPoint;
    [SerializeField]
    int maxTime;

    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .Take(1)
            .Subscribe(_ => StartCoroutine(Move()));

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
}
