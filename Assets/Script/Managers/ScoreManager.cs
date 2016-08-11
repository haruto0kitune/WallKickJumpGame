using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Text text;
    public ReactiveProperty<int> score;

    void Awake()
    {
        score = new ReactiveProperty<int>();
    }

    void Start()
    {
        score.SubscribeToText(text); 
    }
}