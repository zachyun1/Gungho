using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public GameObject teleportLocation;

    GameObject target;


    void FixedUpdate()
    {
        if(target)
        {
            target.transform.position = new Vector2(teleportLocation.transform.position.x,
                                            teleportLocation.transform.position.y);
            target.GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0);
            StartCoroutine(LateCall(2.0f));
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            target = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = null;
        }
    }

    IEnumerator LateCall(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(teleportLocation);
    }

}
