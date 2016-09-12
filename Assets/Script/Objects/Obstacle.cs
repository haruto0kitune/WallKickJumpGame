using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Obstacle : MonoBehaviour
{
    Rigidbody2D _rigidbody2d;
    Vector2 velocityStore;
    float angularVelocityStore;
    float gravityScaleStore;
    [SerializeField]
    float fallingSpeed;

    void Awake()
    {
        _rigidbody2d = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Destroy
        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ =>
            {
                Destroy(gameObject);
            });

        // Speed Limit
        this.ObserveEveryValueChanged(x => _rigidbody2d.velocity.y)
            .Subscribe(_ => _rigidbody2d.velocity = new Vector2(_rigidbody2d.velocity.x, -fallingSpeed));
    }

    public void Pause()
    {
        velocityStore = _rigidbody2d.velocity;
        angularVelocityStore = _rigidbody2d.angularVelocity;
        gravityScaleStore = _rigidbody2d.gravityScale;
        _rigidbody2d.gravityScale = 0f;
        _rigidbody2d.Sleep();
    }

    public void Resume()
    {
        _rigidbody2d.WakeUp();
        _rigidbody2d.velocity = velocityStore;
        _rigidbody2d.angularVelocity = angularVelocityStore;
        _rigidbody2d.gravityScale = gravityScaleStore;
    }
}
