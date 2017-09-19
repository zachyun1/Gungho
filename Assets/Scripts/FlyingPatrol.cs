using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingPatrol : MonoBehaviour {

    public float speed;
    public float duration;
    public bool startReversed = false;

    bool activated = true;
    Vector3 destination;
    Vector3 reverse;

    // Use this for initialization
    void Start()
    {
        if (startReversed)
            ReverseStart();
        else
            Reverse();

    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        if (activated)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, step);
        }
        else
            transform.position = Vector3.MoveTowards(transform.position, reverse, step);

    }

    public void Reverse()
    {
        destination = transform.position + new Vector3(100, 0, 0);
        reverse = transform.position;
        StartCoroutine(LateCall());
        activated = true;
    }

    public void ReverseStart()
    {
        reverse = transform.position + new Vector3(-100, 0, 0);
        destination = transform.position;
        StartCoroutine(LateCall2());
        activated = false;
    }

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(duration);
        activated = false;
        StartCoroutine(LateCall2());
    }

    IEnumerator LateCall2()
    {
        yield return new WaitForSeconds(duration);
        activated = true;
        StartCoroutine(LateCall());
    }
    



}
