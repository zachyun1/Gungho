using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitResources : MonoBehaviour {

    public int health = 100;
    public int maxHealth = 100;

    public void TakeDamage(int value)
    {
        health -= value;
        if(health < 0)
        {
            health = 0;
        }
    }
}
