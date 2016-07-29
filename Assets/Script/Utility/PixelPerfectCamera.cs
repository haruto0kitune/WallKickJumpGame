using UnityEngine;
using System.Collections;

public class PixelPerfectCamera : MonoBehaviour
{
    public const float PixelToUnits = 100.0f;

    // ゲーム内解像度 
    public const int BaseScreenWidth = 270;
    public const int BaseScreenHeight = 480;

    void Awake()
    {
        Camera cam = gameObject.GetComponent<Camera>();
        cam.orthographicSize = BaseScreenHeight / PixelToUnits / 2;

        float baseAspect = (float)BaseScreenHeight / (float)BaseScreenWidth;
        float nowAspect = (float)Screen.height / (float)Screen.width;
        float changeAspect;

        if (baseAspect > nowAspect)
        {
            changeAspect = nowAspect / baseAspect;
            cam.rect = new Rect((1.0f - changeAspect) * 0.5f, 0.0f, changeAspect, 1.0f);
        }
        else
        {
            changeAspect = baseAspect / nowAspect;
            cam.rect = new Rect(0.0f, (1.0f - changeAspect) * 0.5f, 1.0f, changeAspect);
        }
    }
    //void Awake()
    //{
    //    Screen.SetResolution(270, 480, true, 60);
    //    _camera = mainCamera.GetComponent<Camera>();
    //    _camera.orthographicSize = Screen.height / 100 / 2;
    //}
}
