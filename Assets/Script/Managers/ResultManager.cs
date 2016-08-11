using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ResultManager : MonoBehaviour
{
    ScoreManager scoreManager;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text meterText;

    void Awake()
    {
        scoreManager = ScoreManager.Instance;
    }

    void Start()
    {
        scoreManager.score.SubscribeToText(scoreText);
        scoreManager.meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterText);
    }
}
