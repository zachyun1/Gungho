using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    float speed;
    float duration;

    GameObject target;
    Rigidbody2D rigid;

    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();

    }

    public void SetAttributes(GameObject target, float duration, float speed)
    {
        this.target = target;
        this.speed = speed;
        Destroy(gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized * speed;
            rigid.velocity = dir;
            speed += Time.deltaTime / 5.0f;
        }
    }
}
