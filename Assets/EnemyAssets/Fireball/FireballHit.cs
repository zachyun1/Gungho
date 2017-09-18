using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballHit : MonoBehaviour {

    public bool destroyOnHit = true;

    int damage;
    bool hitDealt = false;
    CircleCollider2D m_collider;

	// Use this for initialization
	void Start () {
        m_collider = GetComponent<CircleCollider2D>();
        Physics2D.IgnoreLayerCollision(9, 9, true);
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Physics2D.IgnoreLayerCollision(9, 11, true);
        Destroy(gameObject, 20.0f);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void SetAttributes(int damage)
    {
        this.damage = damage;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        print("ENTERED");
        if (destroyOnHit)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerResources>().TakeDamage(damage);
                Destroy(gameObject);
            }
            if (collision.gameObject.layer == 8)
            {
                Destroy(gameObject);
            }
        }
        else if(!hitDealt)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerResources>().TakeDamage(damage);
                hitDealt = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hitDealt)
        {
            if (collision.gameObject.tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerResources>().TakeDamage(damage);
                hitDealt = true;
            }
        }
    }
}
