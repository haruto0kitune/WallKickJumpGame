using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartButton : MonoBehaviour
{
    void Start()
    {

    }

    public void GameStart()
    {
        SceneManager.LoadScene(0);
        SceneManager.UnloadScene(2);
    }
}
