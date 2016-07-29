using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DebugInformation : MonoBehaviour
{
    [SerializeField]
    Text text1;

    void Start()
    {
        FPSCounter.Current.SubscribeToText(text1); 
    }
}
