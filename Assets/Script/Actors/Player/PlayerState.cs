using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerState : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    Animator animator;

    public ReactiveProperty<bool> isFacingRight;
    public ReactiveProperty<bool> canAirMove;
    public ReactiveProperty<bool> canTurn;
    public ReactiveProperty<bool> isWallKickJumping;
    public ReactiveProperty<bool> canWallKickJump;
    public ReactiveProperty<bool> isFalling;
    public ReactiveProperty<bool> isHookShooting;
    public ReactiveProperty<bool> canHookShoot;
    public ReactiveProperty<bool> isDamaged;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        isFacingRight = new ReactiveProperty<bool>(true);
        canAirMove = this.ObserveEveryValueChanged(x => !isDamaged.Value).ToReactiveProperty(true);
        canTurn = this.ObserveEveryValueChanged(x => !isDamaged.Value).ToReactiveProperty(true);
        canWallKickJump = this.ObserveEveryValueChanged(x => !isDamaged.Value).ToReactiveProperty(); 
        isWallKickJumping = this.ObserveEveryValueChanged(x => animator.GetBool("isWallKickJumping")).ToReactiveProperty();
        isFalling = this.ObserveEveryValueChanged(x => animator.GetBool("isFalling")).ToReactiveProperty();
        isHookShooting = this.ObserveEveryValueChanged(x => animator.GetBool("isHookShooting")).ToReactiveProperty();
        isDamaged = this.ObserveEveryValueChanged(x => animator.GetBool("isDamaged")).ToReactiveProperty();

        // Fall velocity limit
        this.ObserveEveryValueChanged(x => _rigidbody2D.velocity.y)
            .Where(x => x < -1)
            .Subscribe(_ => _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -1f));
    }
}
