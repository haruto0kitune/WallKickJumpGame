using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

namespace WallKickJumpGame.Actors.Player.Basics
{
    public class PlayerTurn : MonoBehaviour
    {
        [SerializeField]
        GameObject Player;
        Animator Animator;
        [SerializeField]
        GameObject buttonManager;
        ButtonManager buttonManagerComponent;
        PlayerState PlayerState;
        SpriteRenderer SpriteRenderer;
        BoxCollider2D[] BoxColliders2D;
        CircleCollider2D[] CircleColliders2D;

        void Awake()
        {
            Animator = Player.GetComponent<Animator>();
            buttonManagerComponent = buttonManager.GetComponent<ButtonManager>();
            PlayerState = Player.GetComponent<PlayerState>();
            SpriteRenderer = Player.GetComponent<SpriteRenderer>();
            BoxColliders2D = Player.GetComponentsInChildren<BoxCollider2D>();
            CircleColliders2D = Player.GetComponentsInChildren<CircleCollider2D>();
        }

        void Start()
        {
            //this.FixedUpdateAsObservable()
                //.Where(x => PlayerState.canTurn.Value)
                //.Where(x => (buttonManagerComponent.isRightButtonDown.Value & !(PlayerState.isFacingRight.Value)) | (buttonManagerComponent.isLeftButtonDown.Value & PlayerState.isFacingRight.Value))
                //.Subscribe(_ => this.Turn());
        }

        public void Turn()
        {
            PlayerState.isFacingRight.Value = !PlayerState.isFacingRight.Value;

            // Turn Sprite
            SpriteRenderer.flipX = !SpriteRenderer.flipX;

            // Turn Collision
            foreach(var i in BoxColliders2D)
            {
                i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
            }

            foreach(var i in CircleColliders2D)
            {
                i.offset = new Vector2(i.offset.x * -1f, i.offset.y);
            }
        }
    }
}