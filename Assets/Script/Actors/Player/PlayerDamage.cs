using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class PlayerDamage : MonoBehaviour
{
    public GameObject player;
    Animator animator;
    ObservableStateMachineTrigger observableStateMachineTrigger;
    AudioSource audioSource;
    Rigidbody2D _rigidbody2D;

    void Awake()
    {
        animator = player.GetComponent<Animator>();
        observableStateMachineTrigger = animator.GetBehaviour<ObservableStateMachineTrigger>();
        audioSource = GetComponent<AudioSource>();
        _rigidbody2D = player.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // Animation
        #region EnterDamage
        observableStateMachineTrigger
            .OnStateEnterAsObservable()
            .Where(x => x.StateInfo.IsName("Base Layer.Damage"))
            .Where(x => player != null)
            .Subscribe(_ =>
            {
                _rigidbody2D.velocity = Vector2.zero;
                audioSource.PlayOneShot(audioSource.clip);
            });
        #endregion
    }
}
