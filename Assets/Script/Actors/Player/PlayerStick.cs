using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStick : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    Rigidbody2D _rigidbody2D;
    BoxCollider2D boxCollider2D;
    PlayerState playerState;
    Transform actors;
    [SerializeField]
    GameObject stickHurtBox;
    BoxCollider2D hurtBox;
    [SerializeField]
    GameObject stickTriggerBox;
    BoxCollider2D triggerBox;

    bool hasDamaged;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        playerState = player.GetComponent<PlayerState>();
        actors = player.transform.parent;
        hurtBox = stickHurtBox.GetComponent<BoxCollider2D>();
        triggerBox = stickTriggerBox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Animation
        #region EnterStick
        observableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stick"))
            .Subscribe(_ => 
            {
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.gravityScale = 0f;
                playerState.canDoubleJump.Value = true;
            });
        #endregion
        #region Stick->WallKickJump
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stick"))
            .Where(x => !PauseManager.isPausing)
            .Where(x => playerState.canWallKickJump.Value)
            //.Where(x => Input.touchCount > 0)
            .Where(x => playerState.isTouching)
            .Subscribe(_ =>
            {
                animator.SetBool("isSticking", false);
                animator.SetBool("isWallKickJumping", true);
                _rigidbody2D.gravityScale = 1f;
                playerState.isTouchingWall.Value = false;
                //player.transform.parent = actors;
            });
        #endregion
        #region Stick->Damage
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stick"))
            .Where(x => hasDamaged)
            .Subscribe(_ =>
            {
                animator.SetBool("isSticking", false);
                animator.SetBool("isDamaged", true);
                _rigidbody2D.gravityScale = 1f;
                playerState.isTouchingWall.Value = false;
                //player.transform.parent = actors;
                hasDamaged = false;
            });
        #endregion
        //#region Stick->Fall
        //observableStateMachineTrigger
        //    .OnStateUpdateAsObservable()
        //    .Where(x => x.StateInfo.IsName("Base Layer.Stick"))
        //    //.Where(x => (playerState.isFacingRight.Value && buttonManagerComponent.isLeftButtonDown.Value) || (!playerState.isFacingRight.Value && buttonManagerComponent.isRightButtonDown.Value))
        //    .Subscribe(_ =>
        //    {
        //        animator.SetBool("isSticking", false);
        //        animator.SetBool("isFalling", true);
        //        _rigidbody2D.gravityScale = 1f;
        //        //TurnOnSticking();
        //    });
        //#endregion

        // Trigger
        hurtBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle" || x.gameObject.tag == "Enemy")
            .Subscribe(_ => hasDamaged = true);

        triggerBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Wall")
            .Do(x => Debug.Log("isTouchingWall"))
            .Subscribe(_ =>
            {
                playerState.isTouchingWall.Value = true;
                //player.transform.parent = _.gameObject.transform;
            });


        //triggerBox.OnTriggerExit2DAsObservable()
        //    .Where(x => x.gameObject.tag == "Wall")
        //    .Subscribe(_ => playerState.isTouchingWall.Value = false);

        // Collision
        this.ObserveEveryValueChanged(x => animator.GetBool("isSticking"))
            .Where(x => x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = true;
                hurtBox.enabled = true;
            });

        this.ObserveEveryValueChanged(x => animator.GetBool("isSticking"))
            .Where(x => !x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = false;
                hurtBox.enabled = false;
            });
    }
}
