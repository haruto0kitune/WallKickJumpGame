using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerFall : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    PlayerState playerState;
    BoxCollider2D boxCollider2D;
    CircleCollider2D circleCollider2D;
    [SerializeField]
    GameObject fallHurtBox;
    BoxCollider2D hurtBox;
    bool hasDamaged;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        playerState = player.GetComponent<PlayerState>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hurtBox = fallHurtBox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Animation
        #region Fall->WallKickJump
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
            .Where(x => !GameObject.Find("PauseManager").GetComponent<PauseManager>().isPausing)
            .Where(x => playerState.canWallKickJump.Value)
            .Where(x => Input.GetMouseButtonDown(0))
            .Where(x => EventSystem.current != null)
            .Where(x => EventSystem.current.currentSelectedGameObject == null)
            .Subscribe(_ =>
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isWallKickJumping", true);
            });
        #endregion
        #region Fall->Damage
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
            .Where(x => hasDamaged)
            .Subscribe(_ =>
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isDamaged", true);
                hasDamaged = false;
            });
        #endregion
        #region Fall->HookShot
        #endregion
        #region Fall->Stick
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
            .Where(x => playerState.isTouchingWall.Value)
            .Subscribe(_ =>
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isSticking", true);
            });
        #endregion
        #region Fall->DoubleJump
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
            .Where(x => playerState.canDoubleJump.Value)
            .Where(x => Input.touchCount > 0)
            .Where(x => EventSystem.current != null)
            .Where(x => EventSystem.current.currentSelectedGameObject == null)
            .Subscribe(_ =>
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isDoubleJumping", true);
                playerState.canDoubleJump.Value = false;
            });
        #endregion
        #region Fall->Stand
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
            .Where(x => playerState.isGrounded.Value)
            .Subscribe(_ =>
            {
                animator.SetBool("isFalling", false);
                animator.SetBool("isStanding", true);
            });
        #endregion

        // Collision
        this.ObserveEveryValueChanged(x => animator.GetBool("isFalling"))
            .Where(x => x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = true;
                circleCollider2D.enabled = true;
                hurtBox.enabled = true;
            });

        this.ObserveEveryValueChanged(x => animator.GetBool("isFalling"))
            .Where(x => !x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = false;
                circleCollider2D.enabled = false;
                hurtBox.enabled = false;
            });

        // Trigger
        hurtBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle" || x.gameObject.tag == "Enemy")
            .Subscribe(_ => hasDamaged = true);
    }
}
