using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;

public class ResetManager : MonoBehaviour 
{
    [SerializeField]
    GameObject game;
    List<GameObject> resets;
    public static List<IReset> resetComponents;

    void Awake ()
    {
        resets = new List<GameObject>();
        resetComponents = new List<IReset>();

        foreach (var item in game.Descendants().Where(x => x.GetComponent<IReset>() != null))
        {
            resets.Add(item);
        }

        foreach (var item in resets)
        {
            resetComponents.Add(item.GetComponent<IReset>());
        }
    }

    void Start () 
    {
	
    }

    public void Reset()
    {
        resets.RemoveAll(x => x == null);
        resetComponents.RemoveAll(x => true);

        foreach (var item in resets)
        {
            resetComponents.Add(item.GetComponent<IReset>());
        }

        foreach (var item in resetComponents)
        {
            item.Reset();
        }
    }
}
