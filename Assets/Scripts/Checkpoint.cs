using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public int checkpointNum;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            GameControl.control.SetActiveCheckpoint(checkpointNum);
            collider.gameObject.GetComponent<PlayerResources>().Revive();
        }
    }
}
