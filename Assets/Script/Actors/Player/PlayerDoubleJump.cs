using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerDoubleJump : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    PlayerState playerState;
    Rigidbody2D _rigidbody2D;
    BoxCollider2D boxCollider2D;
    [SerializeField]
    GameObject doubleJumpHurtBox;
    BoxCollider2D hurtBox;

    Coroutine coroutineStore;
    bool hasDamaged;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        playerState = player.GetComponent<PlayerState>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hurtBox = doubleJumpHurtBox.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Animation
        #region EnterDoubleJump
        observableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
            .Subscribe(_ => coroutineStore = StartCoroutine(DoubleJump()));
        #endregion
        #region DoubleJump->Fall
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
            .Where(x => _rigidbody2D.velocity.y < 0)
            .Subscribe(_ =>
            {
                animator.SetBool("isDoubleJumping", false);
                animator.SetBool("isFalling", true);
            });
        #endregion
        #region DoubleJump->Damage
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
            .Where(x => hasDamaged)
            .Subscribe(_ =>
            {
                animator.SetBool("isDoubleJumping", false);
                animator.SetBool("isDamaged", true);
                hasDamaged = false;
            });
        #endregion
        #region DoubleJump->Stick
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.DoubleJump"))
            .Where(x => playerState.isTouchingWall.Value)
            .Subscribe(_ =>
            {
                animator.SetBool("isDoubleJumping", false);
                animator.SetBool("isSticking", true);
            });
        #endregion

        // Collision
        this.ObserveEveryValueChanged(x => animator.GetBool("isDoubleJumping"))
            .Where(x => x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = true;
                hurtBox.enabled = true;
            });

        this.ObserveEveryValueChanged(x => animator.GetBool("isDoubleJumping"))
            .Where(x => !x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = false;
                hurtBox.enabled = false;
            });

        // Trigger
        hurtBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle" || x.gameObject.tag == "Enemy")
            .Subscribe(_ =>
            {
                hasDamaged = true;
                Cancel();
            });
    }

    IEnumerator DoubleJump()
    {
        Debug.Log("DoubleJump");
        playerState.canAirMove.Value = false;

        if (playerState.isFacingRight.Value)
        {
            var velocity = Utility.PolarToRectangular2D(105, 4f);
            velocity = new Vector2(velocity.x * -1, velocity.y);
            _rigidbody2D.velocity = velocity;
        }
        else
        {
            var velocity = Utility.PolarToRectangular2D(105, 4f);
            _rigidbody2D.velocity = velocity;
        }

        for (int i = 0; i < 8; i++)
        {
            yield return null;
        }
    }

    void Cancel()
    {
        StopCoroutine(coroutineStore);
        playerState.canAirMove.Value = false;
    }
}
