using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLaser : MonoBehaviour
{ 
    public int attackSpeed = 3;
    public GameObject preLaserPrefab;
    public GameObject laserPrefab;
    public AudioClip laserSFX;

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
        if (!firing && target)
        {
            Vector2 destination = target.transform.position;
            Vector2 center = gameObject.transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector2.left, center - destination);
            transform.rotation = rot;
        }
        if(readyToFire && target)
        {
            firing = true;
            readyToFire = false;

            Vector2 destination = target.transform.position;
            Vector2 center = transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector2.left, center - destination);

            
            GameObject projectileGO = Instantiate<GameObject>(preLaserPrefab,
                                             transform.position - Vector3.up / 5, rot);
            Destroy(projectileGO, 2.0f);


            StartCoroutine(LateLaser(2.0f, rot));
        }
    }

    IEnumerator LateLaser(float val, Quaternion rot)
    {
        yield return new WaitForSeconds(val);
        audio.clip = laserSFX;
        audio.Play(0);

        firing = false;

        GameObject projectileLaser = Instantiate<GameObject>(laserPrefab, 
                           transform.position - Vector3.up / 5 - Vector3.forward * 5, rot);
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
