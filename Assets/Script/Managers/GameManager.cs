using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    void Start()
    {
        player.OnBecameInvisibleAsObservable()
            .ThrottleFirstFrame(60)
            .Subscribe(_ =>
            {
                SceneManager.LoadScene(1);
                SceneManager.UnloadScene(0);
            });
    }
}