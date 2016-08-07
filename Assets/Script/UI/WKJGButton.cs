using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class WKJGButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public ReactiveProperty<bool> isButtonDown;
    public string debug;

    [SerializeField]
    Text text;

    void Awake()
    {
        isButtonDown = new ReactiveProperty<bool>();
    }

    void Start()
    {
        this.ObserveEveryValueChanged(x => isButtonDown.Value)
            .Subscribe(_ => Debug.Log(debug + _));

        isButtonDown.SubscribeToText(text);
    }

    public void OnPointerDown(PointerEventData ped)
    {
        isButtonDown.Value = true;
    }

    public void OnPointerUp(PointerEventData ped)
    {
        isButtonDown.Value = false;
    }
}
