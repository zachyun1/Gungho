using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnderPlayer : MonoBehaviour {

    public GameObject tentaclePrefab;
    public GameObject shadowPrefab;
    public int tentacleDamage;
    public int shadowDamage;

    GameObject shadowSpawn;
    GameObject target;
    bool readyToFire = true;
    int bulletsLeft = 0;
    float basePos;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(readyToFire && bulletsLeft > 0)
        {
            StartCoroutine(LateCall());
            bulletsLeft--;
            StartCoroutine(Reload());
        }
	}

    IEnumerator LateCall()
    {
        Vector3 position = target.transform.position;
        position.y = basePos;
        position -= new Vector3(0, 0.8f, 0);

        yield return new WaitForSeconds(0.5f);

        GameObject spawn = Instantiate(tentaclePrefab, position, transform.rotation);
        spawn.GetComponent<FireballHit>().SetAttributes(tentacleDamage);
        Destroy(spawn, 2.5f);
    }

    IEnumerator Reload()
    {
        readyToFire = false;
        yield return new WaitForSeconds(0.33f);
        readyToFire = true;
    }

    public void ShadowAttack(GameObject target, float distance)
    {
        Vector3 position = gameObject.transform.position;
        position += new Vector3(distance, -0.8f, 0);

        GameObject spawn = Instantiate(shadowPrefab, position, transform.rotation);
        spawn.GetComponent<ShadowMechanic>().SetTargetLocation(target);
        spawn.GetComponent<FireballHit>().SetAttributes(shadowDamage);
        Destroy(spawn, 3.0f);
    }

    public void SetAttributes(GameObject target, float basePos, int value)
    {
        bulletsLeft = value;
        this.target = target;
        this.basePos = basePos;
    }

}
