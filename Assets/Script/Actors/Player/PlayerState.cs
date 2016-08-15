using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerState : MonoBehaviour
{
    [SerializeField]
    GameObject groundCheck;
    public LayerMask layerMask;
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
    public ReactiveProperty<bool> isTouchingWall;
    public ReactiveProperty<bool> isSticking;
    public ReactiveProperty<bool> canDoubleJump;
    public ReactiveProperty<bool> isDoubleJumping;
    public ReactiveProperty<bool> isGrounded;

    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        isFacingRight = new ReactiveProperty<bool>(true);
        canAirMove = this.ObserveEveryValueChanged(x => !isDamaged.Value && !isSticking.Value && !isWallKickJumping.Value).ToReactiveProperty(true);
        canTurn = this.ObserveEveryValueChanged(x => !isDamaged.Value && !isWallKickJumping.Value).ToReactiveProperty(true);
        canWallKickJump = this.ObserveEveryValueChanged(x => !isDamaged.Value).ToReactiveProperty(); 
        isWallKickJumping = this.ObserveEveryValueChanged(x => animator.GetBool("isWallKickJumping")).ToReactiveProperty();
        isFalling = this.ObserveEveryValueChanged(x => animator.GetBool("isFalling")).ToReactiveProperty();
        isHookShooting = this.ObserveEveryValueChanged(x => animator.GetBool("isHookShooting")).ToReactiveProperty();
        isDamaged = this.ObserveEveryValueChanged(x => animator.GetBool("isDamaged")).ToReactiveProperty();
        isTouchingWall = new ReactiveProperty<bool>();
        isSticking = this.ObserveEveryValueChanged(x => animator.GetBool("isSticking")).ToReactiveProperty();
        canDoubleJump = new ReactiveProperty<bool>();
        isDoubleJumping = this.ObserveEveryValueChanged(x => animator.GetBool("isDoubleJumping")).ToReactiveProperty();
        isGrounded = this.ObserveEveryValueChanged(x => (bool)Physics2D.Linecast(transform.position, groundCheck.transform.position, layerMask)).ToReactiveProperty();

        isGrounded.Subscribe(_ => Debug.Log(_));
        // Fall velocity limit
        //this.ObserveEveryValueChanged(x => _rigidbody2D.velocity.y)
        //    .Where(x => x < -1)
        //    .Subscribe(_ => _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -1f));
    }
}
