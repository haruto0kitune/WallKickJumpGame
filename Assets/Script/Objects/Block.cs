using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Block : MonoBehaviour
{
    void Start()
    {
        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(this.gameObject));
    }
}
