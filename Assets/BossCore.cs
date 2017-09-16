using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : MonoBehaviour {

    public GameObject boss;

    int health;
    UnitResources healthComp;

    // Use this for initialization
    void Start () {
        healthComp = GetComponent<UnitResources>();
        health = healthComp.health;
	}
	
	// Update is called once per frame
	void Update () {
		if(health != healthComp.health && (health - healthComp.health > 0))
        {
            boss.GetComponent<EnemyAIActor>().TakeDamage(health - healthComp.health);
            health = healthComp.health;
        }
        if(health <= 80)
        {
            foreach(GameObject turr in boss.GetComponent<EnemyAIActor>().turrets)
            {
                Destroy(turr);
            }
            
            
        }
        if(health <= 0)
        {
            GameControl.control.PlayerVictory();
        }
	}

}
