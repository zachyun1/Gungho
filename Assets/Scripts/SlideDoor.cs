using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideDoor : MonoBehaviour {

    public float speed;
    public float duration;

    bool activated = false;
    Vector3 destination;

	// Use this for initialization
	void Start () {
        destination = transform.position + new Vector3(0, -20, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if(activated)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
        }
	}

    public void ActivateDoor()
    {
        StartCoroutine(LateCall());
        activated = true;
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(duration);
        activated = false;
    }

}
