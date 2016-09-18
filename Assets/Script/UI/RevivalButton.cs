using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class RevivalButton : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Revival()
    {
        audioSource.PlayOneShot(audioSource.clip);
        StartCoroutine(_Revival());
    }

    IEnumerator _Revival()
    {
        ScoreManager.Instance.Reset(ScoreManager.Instance.score.Value, ScoreManager.Instance.meter.Value);
        SceneManager.LoadScene("test");
        SceneManager.UnloadScene("result");
        yield return null;

        //GameManager.Instance.player.transform.position = new Vector2(0.97f, ScoreManager.Instance.meter.Value);
    }
}
