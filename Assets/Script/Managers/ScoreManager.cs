using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField]
    GameObject player;
    public Text scoreText;
    public Text meterText;
    public static ReactiveProperty<int> score;
    public static ReactiveProperty<float> meter;
    public static float meterStore;

    private static bool hasInitialized;

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

        // Initialize
        if(!hasInitialized)
        {
            score = new ReactiveProperty<int>();
            meter = new ReactiveProperty<float>();
            hasInitialized = true;
        }
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => player.transform.position.y > meter.Value - meterStore)
            .Subscribe(_ => meter.Value = player.transform.position.y + meterStore);

        score.SubscribeToText(scoreText);
        meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterText);
    }

    public void Reset(int score = 0, float meter = 0f)
    {
        ScoreManager.meterStore = meter;
        ScoreManager.score = new ReactiveProperty<int>(score);
        ScoreManager.meter = new ReactiveProperty<float>(meter);
    }
}