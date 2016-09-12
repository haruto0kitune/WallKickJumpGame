using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{
    [SerializeField]
    GameObject restartManager;
    RestartManager restartManagerComponent;
    [SerializeField]
    GameObject pauseUI;
    AudioSource audioSource;
    [SerializeField]
    Text text;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            text.text = "リトライ";
            text.fontSize = 19;
        }
        else
        {
            text.text = "Restart";
        }

        if (SceneManager.GetActiveScene().name == "test")
        {
            restartManagerComponent = restartManager.GetComponent<RestartManager>();
        }
    }

    public void Restart()
    {
        audioSource.PlayOneShot(audioSource.clip);
        _Restart();
    }

    void _Restart()
    {
        ScoreManager.Instance.Reset();

        if (SceneManager.GetActiveScene().name == "test")
        {
            StartCoroutine(restartManagerComponent.Reset());
            //Destroy(GameObject.Find("EventSystem"));
            //GameObject.Find("PauseUI").SetActive(false);
            //GameObject.Find("Player").transform.position = Vector2.zero;
            //GameObject.Find("PauseManager").GetComponent<PauseManager>().Resume();
            //GameManager.DeleteInstance();
            //ScoreManager.DeleteInstance();
            //Destroy(GameObject.Find("Game"));
            //var prefab = Resources.Load<GameObject>("prefab/game");
            //var game = Instantiate(prefab, Vector2.zero, Quaternion.identity) as GameObject;

            //for (int i = 0; i < 10; i++)
            //{
            //    yield return null;
            //}

            //GameObject.Find("EventSystem").SetActive(false);
            
            //for (int i = 0; i < 10; i++)
            //{
            //    yield return null;
            //}

            //GameObject.Find("EventSystem").SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "result")
        {
            SceneManager.LoadScene("test");
        }
    }
}
