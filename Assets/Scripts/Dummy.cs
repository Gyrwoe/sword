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

    public GameObject deathParticulesPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        if (manager == null) manager = Camera.main.GetComponent<ChallengeManager>();
        health = maxHealth;
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
            PlayDeathAnimation();
            manager.DestroyDummy(this);
        }
    }

    public void AddTargets()
    {
        if(manager == null) manager = Camera.main.GetComponent<ChallengeManager>();
        for (int i = 0; i < manager.defaultTargetCount; i++)
        {
            AddTarget();
        }
    }

    /**
     * Adds a new target to the dummy.
     */
    public void AddTarget()
    {
        Vector3 position = GetTargetPosition();
        if(position != Vector3.zero)
        {
            Target target = Instantiate(targetPrefab, Vector3.zero, this.gameObject.transform.rotation, transform).GetComponent<Target>();
            target.gameObject.transform.localPosition = position;
            target.dummy = this;
            targets.Add(target);
        }
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

    public void PlayDeathAnimation()
    {
        Vector3 position = this.gameObject.transform.position;
        GameObject deathParticules = Instantiate(deathParticulesPrefab, new Vector3(position.x, position.y + 2, position.z), this.gameObject.transform.rotation);
        ParticleSystem particuleSystem = deathParticules.GetComponent<ParticleSystem>();
        particuleSystem.Play();
        // Destroy(deathParticules, particuleSystem.main.duration + particuleSystem.main.startLifetime.constantMax);
    }
}
