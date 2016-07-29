using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class TapDetection : MonoBehaviour
{
    [SerializeField]
    Text text;
    public ReactiveProperty<bool> isButtonDown;

    void Start()
    {
        this.UpdateAsObservable()
            .Where(x => Input.GetMouseButton(0))
            .Select(x => Camera.main.ScreenToWorldPoint(Input.mousePosition))
            .Select(x => Physics2D.OverlapPoint(x))
            .Where(x => x)
            .Where(x => x.gameObject.tag == "LeftButton")
            .Subscribe(_ => isButtonDown.Value = true);

        isButtonDown = this.ObserveEveryValueChanged(x => Input.GetMouseButton(0))
            .Where(x => !x)
            .ToReactiveProperty();

        isButtonDown.SubscribeToText(text);
    }
}
