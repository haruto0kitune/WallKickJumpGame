using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Item : MonoBehaviour
{
    [SerializeField]
    GameObject scoreManager;
    ScoreManager scoreManagerComponent;
    AudioSource audioSource;

    bool mustDestroy;

    void Awake()
    {
        scoreManager = GameObject.Find("ScoreManager");
        scoreManagerComponent = scoreManager.GetComponent<ScoreManager>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        this.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ =>
            {
                ScoreManager.score.Value++;
                audioSource.PlayOneShot(audioSource.clip);
                mustDestroy = true; 
            }).AddTo(this.gameObject);

        this.ObserveEveryValueChanged(x => mustDestroy)
            .Where(x => x)
            .DelayFrame(2)
            .Subscribe(_ => Destroy(this.gameObject));
    }
}
