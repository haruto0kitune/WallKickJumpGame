﻿using UnityEngine;
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
    [SerializeField]
    GameObject fallHurtBox;
    BoxCollider2D hurtBox;
    [SerializeField]
    GameObject buttonManager;
    ButtonManager buttonManagerComponent;
    bool hasDamaged;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        playerState = player.GetComponent<PlayerState>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        hurtBox = fallHurtBox.GetComponent<BoxCollider2D>();
        buttonManagerComponent = buttonManager.GetComponent<ButtonManager>();
    }

    void Start()
    {
        // Animation
        #region Fall->WallKickJump
        observableStateMachineTrigger
            .OnStateUpdateAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Fall"))
            .Where(x => playerState.canWallKickJump.Value)
            //.Where(x => buttonManagerComponent.isJumpButtonDown.Value)
            .Where(x => Input.GetMouseButtonDown(0))
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
                hurtBox.enabled = true;
            });

        this.ObserveEveryValueChanged(x => animator.GetBool("isFalling"))
            .Where(x => !x)
            .Subscribe(_ =>
            {
                boxCollider2D.enabled = false;
                hurtBox.enabled = false;
            });

        // Trigger
        hurtBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Obstacle" || x.gameObject.tag == "Enemy")
            .Subscribe(_ => hasDamaged = true);
    }
}
