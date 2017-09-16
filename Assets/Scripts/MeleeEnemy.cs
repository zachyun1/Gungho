using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyActor {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target && IsEnemyInRange())
        {
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, 0), 
                                                     target.transform.position, 2 * Time.deltaTime);
        }
	}

    private bool IsEnemyInRange()
    {
        if (target != null && (target.transform.position - transform.position).magnitude <= rangeRadius)
        {
            return true;
        }
        return false;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = collider.gameObject;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 
                                                     2 * Time.deltaTime);
        }
    }
}
