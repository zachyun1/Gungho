using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    public int health;
    public int maxHealth;
    public AudioClip deathSound;

    public GameObject bodyObject;

    Color damageColor = new Color(1, 0, 0, 1);
    Color defaultColor;

    void Start()
    {
        defaultColor = bodyObject.GetComponent<SkinnedMeshRenderer>().materials[3].GetColor("_Color");
    }

    public void TakeDamage(int value)
    {
        if(value > 0 && !GameControl.control.getPauseState())
        {
            health -= value;
            StopCoroutine("DamageIndicator");
            StartCoroutine(DamageIndicator(0.2f));
            if (health <= 0)
            {
                health = 0;
                GetComponent<AudioSource>().clip = deathSound;
                GetComponent<AudioSource>().Play(0);
                GameControl.control.PlayerDeath();
            }
        }
    }

    public void TakeHeal(int value)
    {
        if(value > 0)
        {
            health += value;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public void Revive()
    {
        health = maxHealth;
    }

    IEnumerator DamageIndicator(float time)
    {
        bodyObject.GetComponent<SkinnedMeshRenderer>().materials[3].SetColor("_Color", damageColor);
        yield return new WaitForSeconds(time);
        bodyObject.GetComponent<SkinnedMeshRenderer>().materials[3].SetColor("_Color", defaultColor);
    }
}
