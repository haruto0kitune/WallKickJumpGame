using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class DontDestroyObject : MonoBehaviour 
{
    void Awake ()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start () 
    {
	
    }
}
