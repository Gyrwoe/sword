using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * An object that can be hit by weapons and destroyed.
 * When all targets are destroyed, the dummy is destroyed.
 */
public class Target : MonoBehaviour
{
    public Dummy dummy;
    
    public int health;

    public int maxHealth = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /**
     * Applies damage to the target when hit by a weapon.
     */
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Weapon"))
        {
            Debug.Log("TOUCH");
            Weapon weapon = other.gameObject.GetComponent<Weapon>();
            health -= weapon.damage;
            if (health <= 0)
            {
                dummy.DestroyTarget(this);
            }
        }
    }
}
