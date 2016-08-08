using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DebugInformation : MonoBehaviour
{
    [SerializeField]
    Text text1;
    [SerializeField]
    Text text2;
    

    void Start()
    {
        FPSCounter.Current.SubscribeToText(text1);
        GameObject.Find("Player").GetComponent<PlayerState>().canAirMove.SubscribeToText(text2);
    }
}
