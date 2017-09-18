using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour {

    public GameObject door;

    bool activated = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" && !activated)
        {
            activated = true;
            door.GetComponent<SlideDoor>().ActivateDoor();
        }
    }
}
