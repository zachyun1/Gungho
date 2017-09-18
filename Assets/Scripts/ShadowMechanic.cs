using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMechanic : MonoBehaviour {

    public float speed;

    GameObject target;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (target)
            transform.position = Vector3.MoveTowards(transform.position,
                                                     target.transform.position - new Vector3(0,0.8f,0), speed * Time.deltaTime);     
    }

    public void SetTargetLocation(GameObject target)
    {
        this.target = target;
    }
}
