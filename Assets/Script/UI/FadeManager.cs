using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public float speed = 0.01f;  
    float alpha;    
    float red, green, blue;    

    void Start()
    {
        red = GetComponent<Image>().color.r;
        green = GetComponent<Image>().color.g;
        blue = GetComponent<Image>().color.b;
        alpha = 1f;

        StartCoroutine(FadeOn());
    }

    IEnumerator FadeOn()
    {
        for (int i = 0; i < 50; i++)
        {
            GetComponent<Image>().color = new Color(red, green, blue, alpha);
            alpha -= speed;
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 50; i++)
        {
            GetComponent<Image>().color = new Color(red, green, blue, alpha);
            alpha += speed;
            Debug.Log(alpha);
            yield return null;
        }

        SceneManager.LoadScene("title");
    }
}
