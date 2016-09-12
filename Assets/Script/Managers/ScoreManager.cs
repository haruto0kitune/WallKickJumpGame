using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public GameObject player;
    public Text scoreValue;
    public Text meterValue;
    public Text scoreText;
    public Text meterText;
    public static ReactiveProperty<int> score;
    public static ReactiveProperty<float> meter;
    public static float meterStore;
    IDisposable addMeterDisposable;
    IDisposable scoreDisposable;
    IDisposable meterDisposable;

    private static bool hasInitialized;

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

        // Initialize
        if (!hasInitialized)
        {
            score = new ReactiveProperty<int>();
            meter = new ReactiveProperty<float>();

            hasInitialized = true;
        }
    }

    void Start()
    {
        addMeterDisposable = this.UpdateAsObservable()
                                 .Where(x => player != null)
                                 .Where(x => player.transform.position.y > meter.Value - meterStore)
                                 .Subscribe(_ => meter.Value = player.transform.position.y + meterStore);

        scoreDisposable = score.SubscribeToText(scoreValue);
        meterDisposable = meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterValue);
    }

    public void Reset(int score = 0, float meter = 0f)
    {
        ScoreManager.meterStore = meter;
        ScoreManager.score = new ReactiveProperty<int>(score);
        ScoreManager.meter = new ReactiveProperty<float>(meter);
    }
    
    public void Initialize()
    {
        addMeterDisposable.Dispose();
        scoreDisposable.Dispose();
        meterDisposable.Dispose();

        score = new ReactiveProperty<int>();
        meter = new ReactiveProperty<float>();

        addMeterDisposable = this.UpdateAsObservable()
                                 .Where(x => player != null)
                                 .Where(x => player.transform.position.y > meter.Value - meterStore)
                                 .Subscribe(_ => meter.Value = player.transform.position.y + meterStore);

        scoreDisposable = score.SubscribeToText(scoreValue);
        meterDisposable = meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterValue);
    }

    public static void DeleteInstance()
    {
        Instance = null;
    }
}