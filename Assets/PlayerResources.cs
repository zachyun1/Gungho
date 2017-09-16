using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    public int health;
    public int maxHealth;

    bool dead = false;

    public void TakeDamage(int value)
    {
        if(value < 0)
        {
            health -= value;
            if (health < 0)
            {
                health = 0;
                dead = true;
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
}
