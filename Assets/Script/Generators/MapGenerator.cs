using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject prefabOfMapObject;
    [SerializeField]
    GameObject Maps;
    BoxCollider2D TriggerBox;
    readonly float coordinateY = 3.2f;
    public ReactiveProperty<int> count;

    void Awake()
    {
        TriggerBox = GetComponent<BoxCollider2D>();
        count = new ReactiveProperty<int>(1);
    }

    void Start()
    {
        TriggerBox.OnTriggerEnter2DAsObservable()
            .Where(x => x.gameObject.tag == "Player")
            .Subscribe(_ =>
            {
                // Generate Map
                var prefab = Instantiate(prefabOfMapObject, new Vector3(0f, (coordinateY * count.Value), 0f), Quaternion.identity) as GameObject;
                prefab.transform.parent = Maps.transform;
                
                // Add Component BoxCollider2D of enabled trigger.
                var _triggerBox =  this.gameObject.AddComponent<BoxCollider2D>();
                _triggerBox.isTrigger = true;
                _triggerBox.offset = new Vector2(0f, 4.8f * count.Value);
                _triggerBox.size = new Vector2(2.7f, 4.8f);

                count.Value++;
            });
    }
}
