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
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text meterText;
    public ReactiveProperty<int> score;
    public ReactiveProperty<float> meter;

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
        score = new ReactiveProperty<int>();
        meter = new ReactiveProperty<float>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => player.transform.position.y > meter.Value)
            .Subscribe(_ => meter.Value = player.transform.position.y);

        score.SubscribeToText(scoreText);
        meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterText);
    }
}