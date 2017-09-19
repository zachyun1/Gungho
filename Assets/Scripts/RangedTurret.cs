using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTurret : EnemyActor {

    public GameObject projectilePrefab;
    public GameObject fireFrom;
    public float projectileVelocity;

    private bool readyToFire = true;
    UnitResources res;

	// Use this for initialization
	void Start () {
        res = GetComponent<UnitResources>();
	}

	
	// Update is called once per frame
	void Update () {
        if (res.health <= 0)
        {
            res.health = 100;
            StartCoroutine(LateCall(1.0f));
        }
            
        if(readyToFire && target && projectilePrefab)
        {
            Attack();
            StartCoroutine(Reload());
            readyToFire = false;
        }

	}



    private bool IsEnemyInRange()
    {
        if(target != null && (target.transform.position - transform.position).magnitude <= rangeRadius)
        {
            return true;
        }
        return false;
    }

    private void Attack()
    {
        //Get the quaternion rotation from the shooter to the target
        Vector2 destination = target.transform.position;
        Vector2 center = fireFrom.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

        //Instantiate the projectile with the correct angle and position
        GameObject projectile = Instantiate(projectilePrefab, center, rot) as GameObject;
        projectile.GetComponent<FireballHit>().SetAttributes(attackDamage);


        //Get the rigid body 2D and apply a force towards the target with given velocity
        Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
        rigidbody.velocity = (destination - center).normalized * projectileVelocity;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(attackSpeed);
        readyToFire = true;
    }

    IEnumerator LateCall(float time)
    {
        GetComponent<AudioSource>().Play(0);
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
