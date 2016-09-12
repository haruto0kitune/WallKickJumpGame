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

    public bool isTouching;

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

        this.UpdateAsObservable()
            .Where(x => Input.touchCount > 0)
            .Subscribe(_ => 
            {
                Rect rect = new Rect(220, 420, 50, 80);
                var touch = Input.GetTouch(0);
                //Debug.Log("touch: " + touch.position);
                //Debug.Log("xMin: " + rect.xMin);
                //Debug.Log("xMax: " + rect.xMax);
                //Debug.Log("yMin: " + rect.yMin);
                //Debug.Log("yMax: " + rect.yMax);
                //Debug.Log("isTouching: " + isTouching);


                if(!(rect.xMin <= touch.position.x
                && rect.xMax >= touch.position.x
                && rect.yMin <= touch.position.y
                && rect.yMax >= touch.position.y))
                {
                    isTouching = true;
                }
                else
                {
                    isTouching = false;
                }
            });

        this.UpdateAsObservable()
            .Where(x => Input.touchCount <= 0)
            .Subscribe(_ => isTouching = false);
    }
}
