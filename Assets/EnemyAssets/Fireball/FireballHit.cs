﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHit : MonoBehaviour {

    private CircleCollider2D m_collider;

	// Use this for initialization
	void Start () {
        m_collider = GetComponent<CircleCollider2D>();
        Physics2D.IgnoreLayerCollision(9, 9, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Destroy(gameObject, 20.0f);
    }
	
	// Update is called once per frame
	void Update () {

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //collision.gameObject.GetComponent<PlayerResources>().TakeDamage(5);
            Destroy(gameObject);
        }
        if (collision.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
    }
}
