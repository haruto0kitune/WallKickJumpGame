using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerState : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;
    Animator animator;

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
        isWallKickJumping = this.ObserveEveryValueChanged(x => animator.GetBool("isWallKickJumping")).ToReactiveProperty();
        isFalling = this.ObserveEveryValueChanged(x => animator.GetBool("isFalling")).ToReactiveProperty();
        isHookShooting = this.ObserveEveryValueChanged(x => animator.GetBool("isHookShooting")).ToReactiveProperty();
        isDamaged = this.ObserveEveryValueChanged(x => animator.GetBool("isDamaged")).ToReactiveProperty();
    }
}
