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
        if (player.transform.position.y > 0)
        {
            transform.position = new Vector3(0, player.transform.position.y, transform.position.z);
        }

        if (player.transform.position.y < 0)
        {
            transform.position = new Vector3(0, 0, transform.position.z);
        }
	}
}
