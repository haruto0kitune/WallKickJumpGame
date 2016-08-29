using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject player;
    [System.NonSerialized]
    public Animator animator;
    [System.NonSerialized]
    public Rigidbody2D rigidbody2d;
    PlayerState playerState;
    [SerializeField]
    GameObject playerDamage;
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

        Instance = this;
        DontDestroyOnLoad(gameObject);

        animator = player.GetComponent<Animator>();
        rigidbody2d = player.GetComponent<Rigidbody2D>();
        audioSource = playerDamage.GetComponent<AudioSource>();
        playerState = player.GetComponent<PlayerState>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Select(x => audioSource.isPlaying)
            .Pairwise()
            .Where(x => x.Previous && !x.Current)
            .Subscribe(_ => hasFinishedSE = true);

        player.OnBecameInvisibleAsObservable()
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Subscribe(_ => hasBecomeInvisible = true);

        this.ObserveEveryValueChanged(x => (hasFinishedSE && hasBecomeInvisible && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && hasBecomeInvisible))
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Where(x => x)
            .Subscribe(_ => SceneManager.LoadScene("result"));
    }
}