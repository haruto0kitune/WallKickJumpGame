using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class ResultManager : MonoBehaviour
{
    ScoreManager scoreManager;
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text meterText;
    [SerializeField]
    Text scoreRankText;
    [SerializeField]
    Text meterRankText;

    void Awake()
    {
        scoreManager = ScoreManager.Instance;
    }

    void Start()
    {
        ScoreManager.score.SubscribeToText(scoreText);
        ScoreManager.meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterText);
    }
}
