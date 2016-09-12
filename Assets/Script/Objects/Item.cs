using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Item : MonoBehaviour, IPause, IReset
{
    [SerializeField]
    GameObject scoreManager;
    ScoreManager scoreManagerComponent;
    AudioSource audioSource;
    Animator animator;

    bool mustDestroy;

    void Awake()
    {
        scoreManager = GameObject.Find("ScoreManager");
        scoreManagerComponent = scoreManager.GetComponent<ScoreManager>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        PauseManager.pausers.Add(this);
        ResetManager.resetComponents.Add(this);

        this.ObserveEveryValueChanged(x => animator)
            .Where(x => x == null)
            .Subscribe(_ => Debug.Log("animator null"));

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
            .Subscribe(_ =>
            {
                PauseManager.pausers.Remove(this);
                Destroy(this.gameObject);
            });
    }

    public void Pause()
    {
        PauseManager.pausers.RemoveAll(x => x == null);
        if (animator != null)
        {
            animator.speed = 0;
        }
    }

    public void Resume()
    {
        PauseManager.pausers.RemoveAll(x => x == null);
        animator.speed = 1;
    }

    public void Reset()
    {
        scoreManager = GameObject.Find("ScoreManager");
        scoreManagerComponent = scoreManager.GetComponent<ScoreManager>();
        //audioSource = GetComponent<AudioSource>();
/*        animator = GetComponent<Animator>()*/;
    }

    void OnDestroy()
    {
        PauseManager.pausers.Remove(this);
    }
}
