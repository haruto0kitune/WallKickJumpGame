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
    public ReactiveProperty<int> score;
    public ReactiveProperty<float> meter;
    public static float meterStore;
    private AudioSource audioSource;
    
    private static bool hasInitialized;
    private int meterCounter = 1;

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

        // Initialize
        score = new ReactiveProperty<int>();
        meter = new ReactiveProperty<float>();

        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => player != null)
            .Where(x => player.transform.position.y > meter.Value - meterStore)
            .Subscribe(_ => meter.Value = player.transform.position.y + meterStore);

        // Blink scoreValue text
        score
            .Where(x => x % 10 == 0 && x >= 10)
            .Do(x => audioSource.PlayOneShot(audioSource.clip))
            .Do(x => scoreValue.enabled = false)
            .DelayFrame(5)
            .Do(x => scoreValue.enabled = true)
            .DelayFrame(5)
            .Do(x => scoreValue.enabled = false)
            .DelayFrame(5)
            .Do(x => scoreValue.enabled = true)
            .DelayFrame(5)
            .Do(x => scoreValue.enabled = false)
            .DelayFrame(5)
            .Do(x => scoreValue.enabled = true)
            .DelayFrame(5)
            .Subscribe(_ => { });

        // Blink meterValue text
        // over 10 meter
        meter
            .Where(x => x >= 10 * meterCounter)
            .ThrottleFirstFrame(60)
            .Do(x => audioSource.PlayOneShot(audioSource.clip))
            .Do(x => meterValue.enabled = false)
            .DelayFrame(5)
            .Do(x => meterValue.enabled = true)
            .DelayFrame(5)
            .Do(x => meterValue.enabled = false)
            .DelayFrame(5)
            .Do(x => meterValue.enabled = true)
            .DelayFrame(5)
            .Do(x => meterValue.enabled = false)
            .DelayFrame(5)
            .Do(x => meterValue.enabled = true)
            .DelayFrame(5)
            .Subscribe(_ => meterCounter++);

        score.SubscribeToText(scoreValue);
        meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterValue);
    }

    public void Reset(int score = 0, float meter = 0f)
    {
        ScoreManager.meterStore = meter;
        this.score = new ReactiveProperty<int>(score);
        this.meter = new ReactiveProperty<float>(meter);
    }
    
    public void Initialize()
    {
        Instance = null;
    }
}