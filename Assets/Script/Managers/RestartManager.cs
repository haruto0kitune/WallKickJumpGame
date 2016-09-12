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
    PauseManager pauseManagerComponent;
    [SerializeField]
    GameObject scoreManager;
    ScoreManager scoreManagerComponent;

    public static bool isResetting;

    void Awake ()
    {
        pauseManagerComponent = pauseManager.GetComponent<PauseManager>();
        scoreManagerComponent = scoreManager.GetComponent<ScoreManager>();
    }

    void Start () 
    {
	
    }

    public IEnumerator Reset()
    {
        yield return new WaitForEndOfFrame();
        obstacles.Descendants().Destroy();
        maps.Descendants().Destroy();
        Destroy(GameObject.Find("Player"));
        GameObject.Find("ResetManager").GetComponent<ResetManager>().Reset();
        isResetting = true;
        var prefab = Resources.Load("prefab/Actors/Player/Player");
        var player = Instantiate(prefab, GameObject.Find("Actors").transform) as GameObject;
        player.name = "Player";
        //var prefab2 = Resources.Load("prefab/Actors/Player/Damage");
        //var playerDamage = Instantiate(prefab2, player.transform) as GameObject;
        //playerDamage.GetComponent<PlayerDamage>().player = player;
        //player.transform.position = new Vector2(0.97f, 0f);
        //player.GetComponent<SpriteRenderer>().flipX = false;
        //player.GetComponent<Animator>().Play("Stick");
        //player.transform.parent = GameObject.Find("Actors").transform;
        
        yield return null;

        // Generate Map Object.
        var mapObject = Instantiate(mapObjects[Random.Range(0, mapObjects.Length)], Vector2.zero, Quaternion.identity) as GameObject;
        //var mapObject2 = Instantiate(mapObjects[Random.Range(0, mapObjects.Length)], Vector2.zero, Quaternion.identity) as GameObject;
        // Add Component BoxCollider2D of enabled trigger.
        var _triggerBox = mapObject.AddComponent<BoxCollider2D>();
        _triggerBox.isTrigger = true;
        _triggerBox.offset = new Vector2(0f, 4.8f);
        _triggerBox.size = new Vector2(2.7f, 4.8f);
        mapObject.transform.parent = maps.transform;

        GameObject.Find("PauseUI").SetActive(false);
        mainCamera.transform.position = new Vector3(0f, 0f, -10f);
        subCamera.transform.position = new Vector3(0f, 0f, -10f);
        mainCamera.GetComponent<MainCamera>().player = player;
        subCamera.GetComponent<MainCamera>().player = player;
        GameObject.Find("ObstacleGenerator").GetComponent<ObstacleGenerator>().player = player;
        //scoreManagerComponent.player = player;
        ScoreManager.Instance.player = player;
        ScoreManager.Instance.Initialize();
        GameManager.Instance.player = player;
        GameManager.Instance.mainCamera = Camera.main.gameObject.GetComponent<MainCamera>();
        GameManager.Instance.Initialize();
        foreach (var item in player.Descendants().Where(x => x.name == "Damage"))
        {
            GameManager.Instance.playerDamage = item; 
        }

        PauseManager.pausers.RemoveAll(x => x == null);

        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        isResetting = false;
    }
}
