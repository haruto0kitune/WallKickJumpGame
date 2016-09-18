using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ObstacleGenerator : MonoBehaviour, IPause
{
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
        this.UpdateAsObservable()
            .Where(x => !GameObject.Find("PauseManager").GetComponent<PauseManager>().isPausing)
            .Where(x => canGenerate)
            .ThrottleFirstFrame(generateDurationFrame)
            .Subscribe(_ => 
            {
                var coordinateX = Random.Range(-0.87f, 0.87f);
                var prefab = Instantiate(Obstacle, new Vector3(coordinateX, Camera.main.transform.position.y + 3.2f, 0f), Quaternion.identity) as GameObject;
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
