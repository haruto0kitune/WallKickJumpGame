using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    PlayerState playerState;
    [SerializeField]
    GameObject playerDamage;
    AudioSource audioSource;

    bool hasFinishedSE;
    bool hasBecomeInvisible;

    void Awake()
    {
        audioSource = playerDamage.GetComponent<AudioSource>();
        playerState = player.GetComponent<PlayerState>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Select(x => audioSource.isPlaying)
            .Pairwise()
            .Where(x => x.Previous && !x.Current)
            .Subscribe(_ => hasFinishedSE = true);

        player.OnBecameInvisibleAsObservable()
            .Subscribe(_ => hasBecomeInvisible = true);

        this.ObserveEveryValueChanged(x => (hasFinishedSE && hasBecomeInvisible && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && hasBecomeInvisible))
            .Where(x => x)
            .Subscribe(_ =>
            {
                SceneManager.LoadScene(1);
                SceneManager.UnloadScene(0);
            });
    }
}