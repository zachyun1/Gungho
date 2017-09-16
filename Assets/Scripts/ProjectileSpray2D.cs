﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpray2D : MonoBehaviour {

    public int numberOfProjectiles;
    public GameObject prefab;
    public float projectileVelocity;
    public float fireDelay;
    public float attackSpeed;
    public float angle;

    Transform fireFrom;
    GameObject target;

    private bool readyToFire = false;

    // Use this for initialization
    void Start () {
        fireFrom = transform;
	}
	
	// Update is called once per frame
	void Update () {
        if (readyToFire && target)
        {
            Attack();
            StartCoroutine(Reload());     
        }   
    }

    IEnumerator Reload()
    {
        readyToFire = false;
        yield return new WaitForSeconds(attackSpeed);
        readyToFire = true;
    }

    public void ManualFire(GameObject target)
    {
        this.target = target;
        Attack();
    }

    public void SetAutoFireState(bool state)
    {
        readyToFire = state;
    }

    void Attack()
    {
        //Get the quaternion rotation from the shooter to the target
        Vector2 destination = target.transform.position;
        Vector2 center = fireFrom.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

        int spread = numberOfProjectiles % 2 == 0 ? numberOfProjectiles / 2 : (numberOfProjectiles - 1) / 2;
        if (numberOfProjectiles % 2 == 0)
        {
            fireSprayEven(spread, center, rot);
        }
        else
        {
            fireSprayOdd(spread, center, rot);
        }
    }

    void fireSprayEven(int spread, Vector2 center, Quaternion rot)
    {
        for (int i = -spread; i < spread + 1; i++)
        {
            //Skip the middle projectile
            if (i == 0)
                continue;

            GameObject projectile = Instantiate(prefab, center, rot) as GameObject;

            //Since there is no middle projectile, correct the angles of each other projectile
            if (Mathf.Abs(i) == 1)
            {
                projectile.transform.Rotate(0, 0, angle / 2 * i);
            }
            else
            {
                if(i > 0)
                    projectile.transform.Rotate(0, 0, angle * i - (angle / 2));
                else
                    projectile.transform.Rotate(0, 0, angle * i + (angle / 2));
            }

            //Get the rigid body and apply a force towards the target with given velocity
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
            Vector2 spreadVec = projectile.transform.rotation * Vector2.left;
            rigidbody.velocity = spreadVec * projectileVelocity;
        }
    }

    void fireSprayOdd(int spread, Vector2 center, Quaternion rot)
    {
        for (int i = -spread; i < spread + 1; i++)
        {

            GameObject projectile = Instantiate(prefab, center, rot) as GameObject;
            projectile.transform.Rotate(0, 0, angle * i);


            //Get the rigid body and apply a force towards the target with given velocity
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
            Vector2 spreadVec = projectile.transform.rotation * Vector2.left;
            rigidbody.velocity = spreadVec * projectileVelocity;
        }
    }

    Vector2 getProjectilePosition(Vector2 center, int count, float r)
    {
        Vector2 position;
        float ang = 45 / numberOfProjectiles * count;
        position.x = center.x + r * Mathf.Sin(ang * Mathf.Deg2Rad);
        position.y = center.y + r * Mathf.Cos(ang * Mathf.Deg2Rad);
        return position;
    }

}
