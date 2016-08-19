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
        scoreManager.score.SubscribeToText(scoreText);
        scoreManager.meter.Select(x => Mathf.Floor(x * 100) / 100).SubscribeToText(meterText);

        //this.ObserveEveryValueChanged(x => SceneManager.GetActiveScene())
        //    .Where(x => x.name == "result")
        //    .Subscribe(_ => 
        //    {
        //        NCMBObject meterRanking = new NCMBObject("meterRanking");
        //        NCMBObject scoreRanking = new NCMBObject("scoreRanking");

        //        meterRanking["meter"] = scoreManager.meter.Value;
        //        scoreRanking[""]
        //    });
    }
}
