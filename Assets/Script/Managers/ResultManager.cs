using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;
using NCMB;

public class ResultManager : MonoBehaviour
{
    [SerializeField]
    Text scoreText;
    [SerializeField]
    Text meterText;

    void Awake()
    {
    }

    void Start()
    {
        ScoreManager.Instance.score.SubscribeToText(scoreText);
        ScoreManager.Instance.meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterText);
    }
}
