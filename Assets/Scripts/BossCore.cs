using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCore : MonoBehaviour {

    public GameObject boss;
    public GameObject stageThreeStructures;
    public int stageThreeThreshold = 10;

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
            health = healthComp.health;
        }
        if (!bossComp.AreTurretsDead() && !stageTwo)
        {
            Color col = GetComponent<SpriteRenderer>().color;
            col.a = 255;
            GetComponent<SpriteRenderer>().color = col;

            GetComponent<CapsuleCollider2D>().enabled = true;

            bossComp.SetWeaponStates(true, true, true);
            stageTwo = true;
        }
        if(health <= stageThreeThreshold && !stageThree)
        {
            GetComponent<Animator>().SetTrigger("CoreDeath");
            boss.GetComponent<Animator>().SetTrigger("StageThree");

            GetComponent<AudioSource>().Play(0);

            bossComp.SetStageThree();
            Destroy(stageThreeStructures);
            stageThree = true;
            StartCoroutine(LateCall());
        }
	}

    IEnumerator LateCall()
    {
        yield return new WaitForSeconds(3.0f);
        
        Destroy(gameObject);
    }

}
