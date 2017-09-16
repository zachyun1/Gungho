using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire2D : MonoBehaviour
{
    public GameObject prefab;
    public GameObject fireFrom;
    public float projectileVelocity;
    public int bulletClip = 0;
    public float attackSpeed;
    public int damage;

    private bool readyToFire = true;
    private int bulletsLeft = 0;
    GameObject target;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(readyToFire && bulletsLeft > 0 && target)
        {
            Attack();
            bulletsLeft--;
            StartCoroutine(Reload());
            
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
        //Get the quaternion rotation from the shooter to the target
        Vector2 destination = target.transform.position;
        Vector2 center = fireFrom.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

        //Instantiate the projectile with the correct angle and position
        GameObject projectile = Instantiate(prefab, center, rot) as GameObject;
        projectile.GetComponent<FireballHit>().SetAttributes(damage);


        //Get the rigid body 2D and apply a force towards the target with given velocity
        Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
        rigidbody.velocity = (destination - center).normalized * projectileVelocity;
    }

    public void SetBullets(GameObject target)
    {
        bulletsLeft = bulletClip;
        this.target = target;
    }
}
