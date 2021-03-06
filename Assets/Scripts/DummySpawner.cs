using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/**
 * Dynamically spawns dummies.
 * Dummies are spawned inside of a circle around the center of the playing area. They cannot spawn close to each other
 * or to the player, or outside of the radius of the playing area.
 */
public class DummySpawner : MonoBehaviour
{
	/**
	 * The list of currently active dummies.
	 */
	public List<Dummy> dummies = new List<Dummy>();
	
	/**
	* The maximum number of dummies before the game stops spawning more.
	*/
	public int maxDummies = 5;

	/**
	* The average number of dummies that should spawn per second.
	*/
	public float dummiesPerSecond = 1;

	/**
	* The minimum distance dummies can spawn away from the player.
	*/
	public float playerRadius = 4;
	
	/**
	* The minimum distance dummies can spawn away from each other.
	*/
	public float dummyRadius = 2.5f;

	/**
	* The size of the playable area, dummies only spawn inside of the playable area.
	*/
	public float playableAreaRadius = 5;

	/**
	* The center of the playable area.
	*/
	public GameObject playableAreaCenterObject;
	
	/**
	* The transform of the center of the playable area.
	*/
	public Transform playableAreaCenter;

	public GameObject player;

	private Transform _playerTransform;

	/**
	 * The prefab containing the scripts and physics components of the dummy.
	 */
	public GameObject dummyBase;
	
	/**
	 * The dummy prefab to spawn, containing the dummy model.
	 */
	public GameObject dummyPrefab;
	
	/**
	 * The visual variants of the dummy prefab.
	 */
	public List<GameObject> dummyVariationPrefabs;

	/**
	 * Why is this even a thing?
	 */
	public GameObject dummyChefPrefab;

	/**
	 * Just a lot of croissants.
	 */
	public GameObject dummyChefParticles;

	/**
	 * Material for targets
	 */
	public Material targetMaterial;

	/**
	 * The position on the Y axis to spawn the prefab at.
	 */
	public float defaultDummyY = -0.9f;

	/**
	 * The maximum number of failed spawning attempts authorized per frame.
	 */
	public int maxSpawnAttempts = 50;

	/**
	 * Whether the spawner is actively running or not.
	 */
	public bool running = false;

	/**
	 * Probability to spawn a variation of the default prefab
	 */
	public float variationChance = 0.3f;

	/**
	 * Dummies spawned will be scaled by this factor.
	 */
    public float scalingFactor;

	// Start is called before the first frame update
	void Start()
	{
		_playerTransform = player.GetComponent<Transform>();
        playableAreaCenter = playableAreaCenterObject.GetComponent<Transform>();

    }

	// Update is called once per frame
	void Update()
	{
		if (running)
		{
			TrySpawn();
		}
	}

	/**
	* Spawns a dummy if one should be spawned this frame.
	*/
	public void TrySpawn()
	{
		if (ShouldDummySpawn())
		{
			Vector3 position = GenerateDummyPosition(maxSpawnAttempts);
			if (!position.Equals(Vector3.zero)) // Zero indicates that the spawn algorithm failed to find a position
			{
				SpawnDummy(position);
			}
		}
	}

	/**
	* Checks if a dummy can be spawned this frame. If it can, has a chance to return true.
	*/
	public bool ShouldDummySpawn()
	{
		if (dummies.Count < maxDummies)
		{
			float probability = Time.deltaTime * dummiesPerSecond;
			if (Random.Range(0.0f, 1.0f) <= probability)
			{
				return true;
			}
		}
		return false;
	}

	/**
	 * Tries to generate valid coordinates for a dummy to spawn at.
	 * Returns zero if fails.
	 */
	public Vector3 GenerateDummyPosition(int remainingTries)
	{
		// If the algorithm cannot find a viable position fast enough, stop calculations
		if (remainingTries == 0)
		{
			Debug.Log("Failed spawn");
			return Vector3.zero;
		}
		
		Vector3 playableAreaCenterPosition = playableAreaCenter.position;
		Vector3 playerPosition = _playerTransform.position;
		playerPosition.y = 0; // We only work in the xz plane for simplicity
		
		// Generate coordinates inside the playable area
		// The playable area is considered circular so the coordinates must respect this constraint
		// After generating x, an imaginary circle of random radius is used to apply the equation of the circle and find y
		float radius = Random.Range(0.0f, playableAreaRadius);
		float x = Random.Range(playableAreaCenterPosition.x - radius, playableAreaCenterPosition.x + radius);
		float z = (float)(Math.Sqrt(Math.Pow(radius, 2) - Math.Pow(x - playableAreaCenterPosition.x, 2)) + playableAreaCenterPosition.z);
		// A symmetry is applied to avoid having points from only half of the circle
		z = Random.value < .5 ? z : z - (2 * (z - playableAreaCenterPosition.z));
		Vector3 proposedPosition = new Vector3(x, 0, z);
		
		// Retry if it is in the FOV of the player and generate new coordinates
		// But if the algorithm is struggling to find a viable position, accept this position
		if (IsInCameraView(proposedPosition) && remainingTries > maxSpawnAttempts/4) return GenerateDummyPosition(--remainingTries);
		
		// Retry if it is too close to the player and generate new coordinates
		if (Vector3.Distance(proposedPosition, playerPosition) < playerRadius) return GenerateDummyPosition(--remainingTries);

		// Retry if it is too close to any given dummy
		foreach (var dummy in dummies)
		{
			Vector3 dummyPosition = dummy.gameObject.GetComponent<Transform>().position;
			dummyPosition.y = 0;
			if (Vector3.Distance(proposedPosition, dummyPosition) < dummyRadius) return GenerateDummyPosition(--remainingTries);
		}

		return proposedPosition;
	}

	/**
	 * Returns true if placing a dummy at the provided position would make it visible by the camera.
	 */
	public bool IsInCameraView(Vector3 position)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		Bounds bounds = new Bounds(position, new Vector3(dummyRadius*2, dummyRadius*2, dummyRadius*2));
		return GeometryUtility.TestPlanesAABB(planes, bounds);
	}

	/**
	* Spawns a single dummy at the given position.
	*/
	public void SpawnDummy(Vector3 position)
	{
		position.y = defaultDummyY;
		
		// Turns the dummy towards the player (very spooky!!)
		Vector3 toPlayer = _playerTransform.position - position;
		toPlayer.y = 0;

		GameObject spawnedDummyBase = Instantiate(dummyBase, position, Quaternion.LookRotation(toPlayer));
		spawnedDummyBase.layer = 0;
        Dummy dummy = spawnedDummyBase.GetComponent<Dummy>();
        
        // Choosing the model
		
		GameObject chosenPrefab;
		
		// Determine if the default dummy should be spawned or a random variation
		if (Random.Range(0.0f, 1.0f) < variationChance)
		{
			int index = Random.Range(0, dummyVariationPrefabs.Count + 1);
			if (index == dummyVariationPrefabs.Count)
			{
				chosenPrefab = dummyChefPrefab;
				dummy.deathParticulesPrefab = dummyChefParticles;
			}
			else chosenPrefab = dummyVariationPrefabs[index];
		}
		else chosenPrefab = dummyPrefab;
		
		GameObject model = Instantiate(chosenPrefab, position, Quaternion.LookRotation(toPlayer), spawnedDummyBase.transform);
		model.layer = 0;
		
		dummies.Add(dummy);

        dummy.AddTargets(targetMaterial);
        spawnedDummyBase.transform.localScale = new Vector3(scalingFactor, scalingFactor, scalingFactor);
    }

    /**
	 * Deletes the given dummy
	 */
    public void RemoveDummy(Dummy dummy)
	{
		Destroy(dummy.gameObject);
		dummies.Remove(dummy);
	}

	/**
	 * Deletes all dummies.
	 */
	public void RemoveAllDummies()
	{
		while (dummies.Count > 0)
		{
			Dummy dummy = dummies[0];
			Destroy(dummy.gameObject);
			dummies.Remove(dummy);
		}
	}
}