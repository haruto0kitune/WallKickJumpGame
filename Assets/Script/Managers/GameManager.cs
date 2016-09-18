using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    [System.NonSerialized]
    public Animator animator;
    [System.NonSerialized]
    public Rigidbody2D rigidbody2d;
    PlayerState playerState;
    public GameObject playerDamage;
    AudioSource audioSource;

    public GameObject game;
    [System.NonSerialized]
    public Transform transformOnDamaged;
    [System.NonSerialized]
    public Vector2 previousVelocity;
    [System.NonSerialized]
    public Scene sceneStore;
    [System.NonSerialized]
    public AnimatorStateInfo previousAnimatorStateInfo;

    bool hasFinishedSE;
    bool hasBecomeInvisible;

    void Awake()
    {
        // Singleton
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    IEnumerator Start()
    {
        animator = player.GetComponent<Animator>();
        rigidbody2d = player.GetComponent<Rigidbody2D>();
        audioSource = playerDamage.GetComponent<AudioSource>();
        playerState = player.GetComponent<PlayerState>();

        yield return null;

        this.UpdateAsObservable()
            .Select(x => audioSource.isPlaying)
            .Pairwise()
            .Where(x => x.Previous && !x.Current)
            .Subscribe(_ => hasFinishedSE = true);

        this.ObserveEveryValueChanged(x => (hasFinishedSE && !playerState.isVisible && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && !playerState.isVisible))
            .Where(x => x)
            .Subscribe(_ => SceneManager.LoadScene("result"))
            .AddTo(playerState);
    }

    public void Initialize()
    {
        Instance = null;
    }
}