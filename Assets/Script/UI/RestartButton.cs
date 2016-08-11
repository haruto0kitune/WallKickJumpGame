using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartButton : MonoBehaviour
{ 
    void Start()
    {

    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
