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
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        else
        {
            if (player.transform.position.y > transform.position.y)
            {
                transform.position = new Vector3(0, player.transform.position.y, transform.position.z);
            }
        }

        //Debug.Log("hasExited: " + hasExited);
    }

    void OnTriggerEnter2D(Collider2D collider2d)
    {
        hasExited = false;
    }

    void OnTriggerExit2D(Collider2D collider2d)
    {
        hasExited = true;
    }
}
