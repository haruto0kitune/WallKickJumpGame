using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ObstacleGenerator : MonoBehaviour, IPause
{
    public GameObject player;
    [SerializeField]
    GameObject Obstacle;
    [SerializeField]
    GameObject Obstacles;
    [SerializeField]
    int generateDurationFrame;
    bool canGenerate;

    void Awake()
    {
        canGenerate = true;
    }

    void Start()
    {
        this.ObserveEveryValueChanged(x => player)
            .Where(x => player == null)
            .Subscribe(_ => player = GameObject.Find("Player"));

        this.UpdateAsObservable()
            .Where(x => player != null)
            .Where(x => !GameObject.Find("PauseManager").GetComponent<PauseManager>().isPausing)
            .Where(x => canGenerate)
            .ThrottleFirstFrame(generateDurationFrame)
            .Subscribe(_ => 
            {
                var coordinateX = Random.Range(-0.87f, 0.87f);
                var prefab = Instantiate(Obstacle, new Vector3(coordinateX, player.transform.position.y + 3.2f, 0f), Quaternion.identity) as GameObject;
                prefab.transform.parent = Obstacles.transform;

            });
    }

    public void Pause()
    {
        canGenerate = false;
    }

    public void Resume()
    {
        canGenerate = true;
    }
}
