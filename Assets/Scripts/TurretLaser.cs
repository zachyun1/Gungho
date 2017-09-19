using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour
{ 
    public int attackSpeed = 3;
    public GameObject preLaserPrefab;
    public GameObject laserPrefab;
    public AudioClip laserSFX;
    public bool shouldRotate = true;

    AudioSource audio;
    GameObject target;
    bool firing = false;
    bool readyToFire = true;


    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Turn the turret to face the target
        if (!firing && target && shouldRotate)
        {
            Turn();
        }
        if(readyToFire && target)
        {
            Attack();
        }
    }

    void Turn()
    {
        Vector2 destination = target.transform.position;
        Vector2 center = gameObject.transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, center - destination);
        transform.rotation = rot;
    }

    void Attack()
    {
        firing = true;
        readyToFire = false;

        Vector2 destination = target.transform.position + new Vector3(0, 1, 0);
        Vector2 center = transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector2.left, center - destination);
        Vector3 adjusted = center + (destination - center).normalized * 19.5f;

        GameObject projectileGO = Instantiate<GameObject>(preLaserPrefab,
                                         adjusted, rot);
        Destroy(projectileGO, 2.0f);


        StartCoroutine(LateLaser(2.0f, rot, adjusted));
    }

    IEnumerator LateLaser(float val, Quaternion rot, Vector3 adjusted)
    {
        yield return new WaitForSeconds(val);
        audio.clip = laserSFX;
        audio.Play(0);

        firing = false;


        GameObject projectileLaser = Instantiate<GameObject>(laserPrefab,
                           adjusted, rot);
        projectileLaser.GetComponent<FireballHit>().SetAttributes(25);
        Destroy(projectileLaser, 0.05f);

        StartCoroutine(Reload(attackSpeed));
    }

    IEnumerator Reload(float time)
    {
        readyToFire = false;
        yield return new WaitForSeconds(time);
        readyToFire = true;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
