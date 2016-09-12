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
    public GameObject playerDamage;
    AudioSource audioSource;
    public MainCamera mainCamera;

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
            DontDestroyOnLoad(gameObject);
        }

        animator = player.GetComponent<Animator>();
        rigidbody2d = player.GetComponent<Rigidbody2D>();
        audioSource = playerDamage.GetComponent<AudioSource>();
        playerState = player.GetComponent<PlayerState>();
        mainCamera = Camera.main.gameObject.GetComponent<MainCamera>();
    }

    void Start()
    {
        this.ObserveEveryValueChanged(x => player)
            .Where(x => x == null)
            .Subscribe(_ => player = GameObject.Find("Player"))
            .AddTo(player);

        this.UpdateAsObservable()
            .Where(x => player != null)
            .Where(x => playerDamage != null)
            .Where(x => !RestartManager.isResetting)
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Select(x => audioSource.isPlaying)
            .Pairwise()
            .Where(x => x.Previous && !x.Current)
            .Subscribe(_ => hasFinishedSE = true)
            .AddTo(player);

        player.OnBecameInvisibleAsObservable()
            .Where(x => player != null)
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Subscribe(_ => hasBecomeInvisible = true)
            .AddTo(player);

        //this.ObserveEveryValueChanged(x => (hasFinishedSE && hasBecomeInvisible && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && hasBecomeInvisible))
        this.ObserveEveryValueChanged(x => (hasFinishedSE && mainCamera.hasExited  && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && mainCamera.hasExited))
            .Where(x => player != null)
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Where(x => x)
            //.Where(x => !RestartManager.isResetting)
            .Do(x => Debug.Log("aheya: " + (hasFinishedSE && mainCamera.hasExited  && playerState.isDamaged.Value)))
            .Subscribe(_ => SceneManager.LoadScene("result"))
            .AddTo(player);
    }

    public static void DeleteInstance()
    {
        Instance = null;
    }

    public void Initialize()
    {
        this.ObserveEveryValueChanged(x => player)
            .Where(x => x == null)
            .Subscribe(_ => player = GameObject.Find("Player"))
            .AddTo(player);

        this.UpdateAsObservable()
            .Where(x => player != null)
            .Where(x => playerDamage != null)
            .Where(x => !RestartManager.isResetting)
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Select(x => audioSource.isPlaying)
            .Pairwise()
            .Where(x => x.Previous && !x.Current)
            .Subscribe(_ => hasFinishedSE = true)
            .AddTo(player);

        player.OnBecameInvisibleAsObservable()
            .Where(x => player != null)
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Subscribe(_ => hasBecomeInvisible = true)
            .AddTo(player);

        //this.ObserveEveryValueChanged(x => (hasFinishedSE && hasBecomeInvisible && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && hasBecomeInvisible))
        this.ObserveEveryValueChanged(x => (hasFinishedSE && mainCamera.hasExited  && playerState.isDamaged.Value) || (hasFinishedSE && playerState.isDamaged.Value) || (!playerState.isDamaged.Value && mainCamera.hasExited))
            .Where(x => player != null)
            .Where(x => SceneManager.GetActiveScene().name == "test")
            .Where(x => x)
            .Do(x => Debug.Log("isResetting: " + RestartManager.isResetting))
            //.Where(x => !RestartManager.isResetting)
            .Do(x => Debug.Log(hasBecomeInvisible))
            .Subscribe(_ => SceneManager.LoadScene("result"))
            .AddTo(player);
    }
}