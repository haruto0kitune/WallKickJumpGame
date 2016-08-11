using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    GameObject mapGenerator;
    [SerializeField]
    GameObject items;
    MapGenerator mapGeneratorComponent;
    Dictionary<int, float> coordinateX;

    void Awake()
    {
        mapGeneratorComponent = mapGenerator.GetComponent<MapGenerator>();

        coordinateX = new Dictionary<int, float>(){
            {0, -0.54f},
            {1, 0.54f}
        };
    }

    void Start()
    {
        //this.ObserveEveryValueChanged(x => mapGeneratorComponent.count)
        mapGeneratorComponent.count
            .Subscribe(_ =>
            {
                List<GameObject> instances = new List<GameObject>();

                for (int i = 0; i < Mathf.FloorToInt(Random.Range(3f, 6f)); i++)
                {
                    instances.Add(Instantiate(prefab, new Vector3(coordinateX[Mathf.FloorToInt(Random.Range(0, 2))], Random.Range(0.16f + (3.2f * mapGeneratorComponent.count.Value), 3.04f + (3.2f * mapGeneratorComponent.count.Value))), Quaternion.identity) as GameObject);
                    
                }

                foreach(var x in instances)
                {
                    if (x.transform.position.x == -0.54f)
                    {
                        x.GetComponent<SpriteRenderer>().flipX = true;
                    }
                    else
                    {
                        x.GetComponent<SpriteRenderer>().flipX = false;
                    }

                    x.transform.parent = items.transform;
                }
            });
    }
}
