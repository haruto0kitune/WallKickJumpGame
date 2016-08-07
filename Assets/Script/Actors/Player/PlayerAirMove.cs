using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerAirMove : MonoBehaviour
{
    [SerializeField]
    GameObject buttonManager;
    ButtonManager buttonManagerComponent;
    [SerializeField]
    GameObject Player;
    Rigidbody2D _rigidbody2D;
    PlayerState playerState;

    void Awake()
    {
        buttonManagerComponent = buttonManager.GetComponent<ButtonManager>();
        _rigidbody2D = Player.GetComponent<Rigidbody2D>();
        playerState = Player.GetComponent<PlayerState>();
    }

    void Start()
    {
        this.FixedUpdateAsObservable()
            .Where(x => playerState.canAirMove.Value)
            .Where(x => buttonManagerComponent.isLeftButtonDown.Value)
            .Subscribe(_ => _rigidbody2D.velocity = new Vector2(-2f, _rigidbody2D.velocity.y));

        this.FixedUpdateAsObservable()
            .Where(x => playerState.canAirMove.Value)
            .Where(x => buttonManagerComponent.isRightButtonDown.Value)
            .Subscribe(_ => _rigidbody2D.velocity = new Vector2(2f, _rigidbody2D.velocity.y));

        this.ObserveEveryValueChanged(x => buttonManagerComponent.isLeftButtonDown.Value)
            .Where(x => playerState.canAirMove.Value)
            .Where(x => !buttonManagerComponent.isLeftButtonDown.Value && !buttonManagerComponent.isRightButtonDown.Value)
            .Subscribe(_ => _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y));

        this.ObserveEveryValueChanged(x => buttonManagerComponent.isRightButtonDown.Value)
            .Where(x => playerState.canAirMove.Value)
            .Where(x => !x)
            .Where(x => !buttonManagerComponent.isLeftButtonDown.Value && !buttonManagerComponent.isRightButtonDown.Value)
            .Subscribe(_ => _rigidbody2D.velocity = new Vector2(0f, _rigidbody2D.velocity.y));
    }
}