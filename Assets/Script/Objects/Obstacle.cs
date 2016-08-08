using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Destroy
        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(gameObject));

        // Speed Limit
        this.ObserveEveryValueChanged(x => _rigidbody2D.velocity.y)
            .Where(x => x < -2f)
            .Subscribe(_ => _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -2f));
    }
}
