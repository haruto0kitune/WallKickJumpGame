using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    [SerializeField]
    GameObject player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (player.transform.position.y > transform.position.y)
        {
            transform.position = new Vector3(0, player.transform.position.y, transform.position.z);
        }
	}
}
