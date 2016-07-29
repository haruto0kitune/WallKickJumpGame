using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerWallKickJump : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Rigidbody2D _rigidbody2D;
    BoxCollider2D boxCollider2D;
    [SerializeField]
    GameObject wallKickJumpHurtBox;
    BoxCollider2D hurtBox;
    [SerializeField]
    GameObject wallKickJumpTriggerBox;
    BoxCollider2D triggerBox;

    void Start()
    {

    }
}
