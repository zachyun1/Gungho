using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {

    public int rangeRadius;
    public int damage;
    public int attackSpeed;
    public float projectileVelocity;
    public GameObject projectilePrefab;
    public AudioClip deathSFX;

    bool readyToFire = true;
    bool deathState = false;
    UnitResources res;
    GameObject target;
    Animator anim;
    AudioSource audio;
    ProjectileSpray2D spray;
    ProjectileFire2D machine;
    TurretLaser laser;

	// Use this for initialization
	void Start () {
        res = GetComponent<UnitResources>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        spray = GetComponent<ProjectileSpray2D>();
        machine = GetComponent<ProjectileFire2D>();
        laser = GetComponent<TurretLaser>();
    }
	
	// Update is called once per frame
	void Update () {
        if (res.health <= 0 && !deathState)
        {
            DeathState();
        }

        if (readyToFire && target && !deathState)
        {
            if(spray)
            {
                spray.ManualFire(target);

            }
            else if(machine)
            {
                machine.SetBullets(target);
            }
            else if(laser)
            {
                laser.SetTarget(target);
            }
            else
            {
                Attack();
            }
            StartCoroutine(Reload());
            readyToFire = false;
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

    private void Attack()
    {
        //Get the quaternion rotation from the shooter to the target
        Vector2 destination = target.transform.position;
        Vector2 center = transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, destination - center);

        //Instantiate the projectile with the correct angle and position
        GameObject projectile = Instantiate(projectilePrefab, center, rot) as GameObject;
        projectile.GetComponent<FireballHit>().SetAttributes(damage);


        //Get the rigid body 2D and apply a force towards the target with given velocity
        Rigidbody2D rigidbody = projectile.GetComponent<Rigidbody2D>();
        rigidbody.velocity = (destination - center).normalized * projectileVelocity;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = collider.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            target = null;
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(attackSpeed);
        readyToFire = true;
    }

    void DeathState()
    {
        deathState = true;
        anim.SetTrigger("Death");

        audio.clip = deathSFX;
        audio.Play(0);

        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
