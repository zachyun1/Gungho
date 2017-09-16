﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileNova2D : MonoBehaviour {

    public int numberOfProjectiles;
    public GameObject prefab;
    public GameObject fireFrom;
    public float projectileVelocity;
    public float fireDelay;
    public float attackSpeed;

    private bool manualFire = false;
    private bool readyToFire = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if(readyToFire)
        //{
        //    Attack();
        //    StartCoroutine(Reload());
        //}
        if(manualFire)
        {
            Attack();
            manualFire = false;
        }
    }

    IEnumerator Reload()
    {
        readyToFire = false;
        yield return new WaitForSeconds(attackSpeed);
        readyToFire = true;
    }

    void Attack()
    {

        Vector2 center = transform.position;
        for (int i = 0; i < numberOfProjectiles; i++)
        {
            //Get the projectile position with the given center
            Vector2 pos = getProjectilePosition(center, i, 0.5f);

            //Get the rotation each projectile should have 
            Quaternion rot = Quaternion.FromToRotation(Vector2.left, pos - center);

            //Instantiate each projectile and give a velocity in each's forward direction
            GameObject projectile = Instantiate(prefab, pos, rot);
            Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
            rigidbody.velocity = (pos - center).normalized * projectileVelocity;
        }
    }

    //Calculate the position of each projectile around the character
    Vector2 getProjectilePosition(Vector2 center, int count, float r)
    {
        Vector2 position;
        float ang = 360 / numberOfProjectiles * count;
        position.x = center.x + r * Mathf.Sin(ang * Mathf.Deg2Rad);
        position.y = center.y + r * Mathf.Cos(ang * Mathf.Deg2Rad);
        return position;
    }

    public void SetAutoFireState(bool state)
    {
        readyToFire = state;
    }

    public void ManualFire()
    {
        manualFire = true;
    }
}
