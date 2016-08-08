using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class PlayerDamage : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    Rigidbody2D _rigidbody2D;
    [SerializeField]
    GameObject buttonManager;
    ButtonManager buttonManagerComponent;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
        buttonManagerComponent = buttonManager.GetComponent<ButtonManager>();
    }

    void Start()
    {
        // Animation
        #region EnterDamage
        observableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Damage"))
            .Subscribe(_ => _rigidbody2D.velocity = Vector2.zero);
        #endregion
    }
}
