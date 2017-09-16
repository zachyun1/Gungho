using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedTurret : EnemyActor {

    public GameObject projectilePrefab;
    public GameObject fireFrom;
    public float projectileVelocity;

    private bool readyToFire = true;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(readyToFire && target)
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


        //Get the rigid body 2D and apply a force towards the target with given velocity
        Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
        rigidbody.velocity = (destination - center).normalized * projectileVelocity;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(attackSpeed);
        readyToFire = true;
    }
}
