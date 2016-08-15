using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class BackgroundOfResult : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        var color = spriteRenderer.color;
        color.a = 128f;
        spriteRenderer.color = color;
    }
}
