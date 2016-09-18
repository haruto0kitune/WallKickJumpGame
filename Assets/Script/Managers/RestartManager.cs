#pragma warning disable 414
using UnityEngine;
using System.Collections;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class RestartManager : MonoBehaviour 
{
    [SerializeField]
    GameObject mainCamera;
    [SerializeField]
    GameObject subCamera;
    [SerializeField]
    GameObject obstacles;
    [SerializeField]
    GameObject maps;
    [SerializeField]
    GameObject[] mapObjects;
    [SerializeField]
    GameObject pauseManager;
    [SerializeField]
    GameObject scoreManager;

    public static bool isResetting;

    void Start () 
    {
	
    }

    public IEnumerator Restart()
    {
        ScoreManager.Instance.Initialize();
        GameManager.Instance.Initialize();
        Destroy(GameObject.Find("EventSystem"));
        Destroy(GameObject.Find("Game"));
        var prefab = Resources.Load<GameObject>("prefab/Game");
        var game = Instantiate(prefab, Vector2.zero, Quaternion.identity);
        game.name = "Game";
        yield return null;
    }
}
