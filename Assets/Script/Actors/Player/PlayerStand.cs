using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerStand : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    Rigidbody2D _rigidbody2d;
    BoxCollider2D boxCollider2d;
    PlayerState playerState;
    [SerializeField]
    GameObject standHurtBox;
    BoxCollider2D hurtBox;

    bool hasDamaged;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        _rigidbody2d = player.GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        playerState = player.GetComponent<PlayerState>();
        hurtBox = standHurtBox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Animation
        #region EnterStand
        observableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Subscribe(_ =>
            {
                _rigidbody2d.velocity = Vector2.zero;
                playerState.canDoubleJump.Value = true;
            });
        #endregion
        #region Stand->WallKickJump
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => Input.touchCount > 0)
            .Subscribe(_ =>
            {
                animator.SetBool("isStanding", false);
                animator.SetBool("isWallKickJumping", true);
            });
        #endregion
        #region Stand->Damage
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Stand"))
            .Where(x => hasDamaged)
            .Subscribe(_ =>
            {
                animator.SetBool("isStanding", false);
                animator.SetBool("isDamaged", true);
                hasDamaged = false;
            });
        #endregion

        // Collision
        this.ObserveEveryValueChanged(x => animator.GetBool("isStanding"))
            .Where(x => x)
            .Subscribe(_ =>
            {
                boxCollider2d.enabled = true;
                hurtBox.enabled = true;
            });

        this.ObserveEveryValueChanged(x => animator.GetBool("isStanding"))
            .Where(x => !x)
            .Subscribe(_ =>
            {
                boxCollider2d.enabled = false;
                hurtBox.enabled = false;
            });

        // Trigger
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle" || x.gameObject.tag == "Enemy")
            .Subscribe(_ => hasDamaged = true);
    }
}
