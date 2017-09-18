using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketHit : MonoBehaviour
{

    public bool destroyOnHit = true;

    int damage;
    bool hitDealt = false;
    CircleCollider2D m_collider;

    // Use this for initialization
    void Start()
    {
        m_collider = GetComponent<CircleCollider2D>();
        Physics2D.IgnoreLayerCollision(14, 10, true);
        Physics2D.IgnoreLayerCollision(14, 11, true);
        Destroy(gameObject, 20.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetAttributes(int damage)
    {
        this.damage = damage;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (destroyOnHit)
        {
            if (collision.gameObject.layer == 9)
            {
                collision.gameObject.GetComponent<UnitResources>().TakeDamage(damage);
                Destroy(gameObject);
            }
            if (collision.gameObject.layer == 8)
            {
                Destroy(gameObject);
            }
        }
    }
}
