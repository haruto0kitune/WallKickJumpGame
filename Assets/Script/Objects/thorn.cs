﻿using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class thorn : MonoBehaviour
{
    void Start()
    {
        // Destroy
        this.OnBecameInvisibleAsObservable()
            .Subscribe(_ => Destroy(gameObject));
    }
}
