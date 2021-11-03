using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public int health;

    public int maxHealth = 10;
    
    public ChallengeManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = Camera.main.GetComponent<ChallengeManager>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Applies damage to the dummy when hit by a weapon.
     */
    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            Weapon weapon = other.gameObject.GetComponent<Weapon>();
            health -= weapon.damage;
            if (health <= 0)
            {
                manager.DestroyDummy(this);
            }
        }
    }
}
