using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

    public GameObject player;
    BoxCollider2D triggerBox;
    public bool hasExited;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if(player != null)
        {
            if (player.transform.position.y > transform.position.y)
            {
                transform.position = new Vector3(0, player.transform.position.y, transform.position.z);
            }
        }
        else
        {
            player = GameObject.Find("Player");
            transform.position = Vector2.zero;
        }
    }
}
