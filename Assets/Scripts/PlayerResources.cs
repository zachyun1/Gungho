using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour {

    public int health;
    public int maxHealth;

    public void TakeDamage(int value)
    {
        if(value > 0)
        {
            health -= value;
            if (health < 0)
            {
                health = 0;
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
}
