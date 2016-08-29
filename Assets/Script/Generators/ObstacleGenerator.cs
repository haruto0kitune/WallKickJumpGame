using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject Obstacle;
    [SerializeField]
    GameObject Obstacles;
    [SerializeField]
    int generateDurationFrame;

    void Start()
    {
        this.UpdateAsObservable()
            .ThrottleFirstFrame(generateDurationFrame)
            .Subscribe(_ => 
            {
                var coordinateX = Random.Range(-0.87f, 0.87f);
                var prefab = Instantiate(Obstacle, new Vector3(coordinateX, player.transform.position.y + 3.2f, 0f), Quaternion.identity) as GameObject;
                prefab.transform.parent = Obstacles.transform;
            });
    }
}
