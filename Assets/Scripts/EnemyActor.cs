using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyActor : MonoBehaviour {

    public int health, maxHealth;
    public int attackDamage;
    public int attackSpeed;

    public float meleeRadius;
    public float rangeRadius;

    protected bool facingRight;
    protected GameObject target;
    protected Animator animator;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
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

    public void TakeDamage(int value)
    {
        health -= value;
        if(health < 0)
        {
            health = 0;
            print("Destroyed");
        }
    }

    protected void DealDamage(GameObject obj, int value)
    {
        
    }


}
