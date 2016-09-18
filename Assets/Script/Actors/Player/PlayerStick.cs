#pragma warning disable 414

using UnityEngine;
using UnityEngine.EventSystems;
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
            .Where(x => !GameObject.Find("PauseManager").GetComponent<PauseManager>().isPausing)
            .Where(x => playerState.canWallKickJump.Value)
            .Where(x => Input.touchCount > 0)
            .Where(x => EventSystem.current != null)
            .Where(x => EventSystem.current.currentSelectedGameObject == null)
            .Subscribe(_ =>
            {
                animator.SetBool("isSticking", false);
                animator.SetBool("isWallKickJumping", true);
                _rigidbody2D.gravityScale = 1f;
                playerState.isTouchingWall.Value = false;
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
                hasDamaged = false;
            });
        #endregion

        // Trigger
        hurtBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle" || x.gameObject.tag == "Enemy")
            .Subscribe(_ => hasDamaged = true);

        triggerBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Wall")
            .ThrottleFirstFrame(1)
            .Subscribe(_ => playerState.isTouchingWall.Value = true);

        triggerBox.OnTriggerExit2DAsObservable()
            .Where(x => x.gameObject.tag == "Wall")
            .ThrottleFirstFrame(1)
            .Subscribe(_ => playerState.isTouchingWall.Value = false);

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
