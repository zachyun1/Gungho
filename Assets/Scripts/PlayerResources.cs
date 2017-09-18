using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    public int health;
    public int maxHealth;
    public AudioClip deathSound;

    public void TakeDamage(int value)
    {
        if(value > 0 && !GameControl.control.getPauseState())
        {
            health -= value;
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
}
