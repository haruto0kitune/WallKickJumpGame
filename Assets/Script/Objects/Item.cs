using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Item : MonoBehaviour
{
    [SerializeField]
    GameObject scoreManager;
    ScoreManager scoreManagerComponent;

    void Awake()
    {
        scoreManager = GameObject.Find("ScoreManager");
        scoreManagerComponent = scoreManager.GetComponent<ScoreManager>();
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ =>
            {
                scoreManagerComponent.score.Value++;
                Destroy(this.gameObject);
            }).AddTo(this.gameObject); 
    }
}
