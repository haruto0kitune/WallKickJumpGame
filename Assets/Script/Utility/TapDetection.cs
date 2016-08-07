using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public class TapDetection : MonoBehaviour
{
    [SerializeField]
    Text text;
    public ReactiveProperty<bool> isButtonDown;
    Touch touchStore;

    void Awake()
    {
        isButtonDown = new ReactiveProperty<bool>();
    }

    void Start()
    {
        this.UpdateAsObservable()
            .SelectMany(x => Input.touches)
            .Select(x => Tuple.Create<Collider2D, Touch>(Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(x.position)), x))
            .Where(x => x.Item1)
            .Where(x => x.Item1.gameObject.tag == tag)
            .Do(x => touchStore = x.Item2)
            .Subscribe(_ => isButtonDown.Value = true);

        this.UpdateAsObservable()
            .Where(x => Input.touchCount > 0)
            .Select(x => touchStore.phase == TouchPhase.Ended)
            .Where(x => x)
            .Subscribe(_ => isButtonDown.Value = !_);

        isButtonDown.SubscribeToText(text);
    }
}