using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : MonoBehaviour {

    public GameObject boss;
    public GameObject stageThreeStructures;

    bool stageThree = false, stageTwo = false;
    EnemyAIActor bossComp;
    int health;
    UnitResources healthComp;

    // Use this for initialization
    void Start () {
        healthComp = GetComponent<UnitResources>();
        bossComp = boss.GetComponent<EnemyAIActor>();
        health = healthComp.health;
	}
	
	// Update is called once per frame
	void Update () {
		if(health != healthComp.health && (health - healthComp.health > 0))
        {
            boss.GetComponent<EnemyAIActor>().TakeDamage(health - healthComp.health);
            health = healthComp.health;
        }
        if(health <= 15 && !stageTwo)
        {
            foreach(GameObject turr in bossComp.turrets)
            {
                Destroy(turr);
            }
            bossComp.SetWeaponStates(true, true, true);
            stageTwo = true;
            
            
        }
        if(health <= 10 && !stageThree)
        {
            //GameControl.control.PlayerVictory();
            boss.GetComponent<Animator>().SetTrigger("StageThree");
            bossComp.SetStageThree();
            Destroy(stageThreeStructures);
            stageThree = true;
        }
	}

}
