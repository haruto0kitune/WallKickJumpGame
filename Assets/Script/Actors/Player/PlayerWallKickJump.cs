using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerWallKickJump : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    SpriteRenderer spriteRenderer;
    BoxCollider2D[] boxCollider2Ds;
    CircleCollider2D[] circleCollider2Ds;
    PlayerState playerState;
    Rigidbody2D _rigidbody2D;
    BoxCollider2D boxCollider2D;
    [SerializeField]
    GameObject wallKickJumpHurtBox;
    BoxCollider2D hurtBox;
    [SerializeField]
    GameObject wallKickJumpTriggerBox;
    BoxCollider2D triggerBox;

    Coroutine coroutineStore;

    bool hasDamaged;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
        boxCollider2Ds = player.GetComponentsInChildren<BoxCollider2D>();
        circleCollider2Ds = player.GetComponentsInChildren<CircleCollider2D>();
        playerState = player.GetComponent<PlayerState>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hurtBox = wallKickJumpHurtBox.GetComponent<BoxCollider2D>();
        triggerBox = wallKickJumpTriggerBox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Animation
        #region EnterWallKickJump
        observableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
            .Subscribe(_ => coroutineStore = StartCoroutine(WallKickJump()));
        #endregion
        #region WallKickJump->Fall
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.WallKickJump"))
            .Where(x => _rigidbody2D.velocity.y < 0)
            .Subscribe(_ =>
            {
                animator.SetBool("isWallKickJumping", false);
                animator.SetBool("isFalling", true);
            });
        #endregion
        #region WallKickJump->Damage
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Laeyr.WallKickJump"))
            .Where(x => hasDamaged)
            .Subscribe(_ =>
            {
                animator.SetBool("isWallKickJumping", false);
                animator.SetBool("isDamaged", true);
                hasDamaged = false;
            });
        #endregion
        #region WallKickJump->HookShooting
        #endregion

        // Collision
        this.ObserveEveryValueChanged(x => animator.GetBool("isWallKickJumping"))
            .Where(x => x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = true;
                hurtBox.enabled = true;
            });

        this.ObserveEveryValueChanged(x => animator.GetBool("isWallKickJumping"))
            .Where(x => !x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = false;
                hurtBox.enabled = false;
            });

        // Trigger
        triggerBox.OnTriggerStay2DAsObservable()
            .Where(x => x.gameObject.tag == "Wall")
            .Subscribe(_ => playerState.canWallKickJump.Value = true);

        triggerBox.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Wall")
            .Subscribe(_ => playerState.canWallKickJump.Value = false);

        hurtBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle")
            .Subscribe(_ => hasDamaged = true);
    }

    IEnumerator WallKickJump()
    {

        playerState.isFacingRight.Value = !playerState.isFacingRight.Value;

        // Turn Sprite
        spriteRenderer.flipX = !spriteRenderer.flipX;

        // Turn Collision
        foreach (var i in boxCollider2Ds)
        {
            i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
        }

        foreach (var i in circleCollider2Ds)
        {
            i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
        }

        playerState.canAirMove.Value = false;

        if (playerState.isFacingRight.Value)
        {
            var velocity = Utility.PolarToRectangular2D(60, 4f);
            //velocity.x *= -1;
            Debug.Log(velocity);
            _rigidbody2D.velocity = velocity;
        }
        else
        {
            var velocity = Utility.PolarToRectangular2D(120, 4f);
            Debug.Log(velocity);
            _rigidbody2D.velocity = velocity;
        }

        for (int i = 0; i < 15; i++)
        {
            yield return null;
        }

        playerState.canAirMove.Value = true;
    }

    void Cancel()
    {
        StopCoroutine(coroutineStore);
        playerState.canAirMove.Value = true;
    }
}
