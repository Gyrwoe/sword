using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dummy : MonoBehaviour
{
    public int health;

    public int maxHealth = 10;
    
    public ChallengeManager manager;
    
    public List<Target> targets = new List<Target>();

    /**
     * The positions at which a new target can be added.
     */
    public List<Vector3> targetPositions;
    
    public GameObject targetPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = Camera.main.GetComponent<ChallengeManager>();
        health = maxHealth;

        for (int i = 0; i < manager.defaultTargetCount; i++)
        {
            AddTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * Destroys the given target.
     */
    public void DestroyTarget(Target target)
    {
        this.targets.Remove(target);
        Destroy(target.gameObject);
        if (targets.Count <= 0)
        {
            manager.DestroyDummy(this);
        }
    }

    /**
     * Adds a new target to the dummy.
     */
    public void AddTarget()
    {
        Target target = Instantiate(targetPrefab, Vector3.zero, this.gameObject.transform.rotation, transform).GetComponent<Target>();
        target.gameObject.transform.localPosition = GetTargetPosition();
        target.dummy = this;
        targets.Add(target);
    }

    /**
     * Selects a random predefined position where a target can be placed.
     * A used position cannot be reused for a target on the same dummy.
     */
    public Vector3 GetTargetPosition()
    {
        if (targetPositions.Count > 0)
        {
            int index = Random.Range(0, targetPositions.Count);
            Vector3 myPosition = targetPositions[index];
            targetPositions.RemoveAt(index);
            return myPosition;
        }
        Debug.Log("Shortage of available target positions.");
        return Vector3.zero;
    }
}
