using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTurret : EnemyActor {

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        print(IsEnemyInRange());
	}



    private bool IsEnemyInRange()
    {
        if(target != null && (target.transform.position - transform.position).magnitude <= rangeRadius)
        {
            return true;
        }
        return false;
    }
}
