using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileLauncher : MonoBehaviour {

    public float projectileSpeed = 1.0f;

    public GameObject prefab;
    public float duration;
    public int damage;

	// Use this for initialization
	void Start () {

	}
	
    public void Attack(GameObject target)
    {
        GameObject projectile = Instantiate(prefab, transform.position, transform.rotation);
        projectile.GetComponent<Homing>().SetAttributes(target, duration, projectileSpeed);
        projectile.GetComponent<FireballHit>().SetAttributes(damage);
    }

}
