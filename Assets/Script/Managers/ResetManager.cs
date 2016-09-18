using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class ResetManager : MonoBehaviour 
{
    public List<IReset> resetComponents;

    void Awake ()
    {
        resetComponents = new List<IReset>();
    }

    void Start () 
    {
	
    }

    public void Reset()
    {
        resetComponents.RemoveAll(x => x == null);

        foreach (var item in resetComponents)
        {
            item.Reset();
        }

        resetComponents = new List<IReset>();
    }
}
