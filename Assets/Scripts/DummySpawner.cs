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
	// /**
	//  * The list of positions for currently active dummies.
	//  */
	// public List<Transform> dummies = new List<Transform>();

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

	public Transform playerTransform;

	/**
	 * The dummy prefab to spawn
	 */
	public GameObject dummyPrefab;

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

	// Start is called before the first frame update
	void Start()
	{
		playerTransform = player.GetComponent<Transform>();
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
			if (!position.Equals(Vector3.zero))
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
		if (remainingTries == 0)
		{
			Debug.Log("Failed spawn");
			return Vector3.zero;
		}
		
		Vector3 playableAreaCenterPosition = playableAreaCenter.position;
		Vector3 playerPosition = playerTransform.position;
		playerPosition.y = 0; // We only work in the xz plane for simplicity
		
		// Generate coordinates inside the playable area
		float x = Random.Range(playableAreaCenterPosition.x - playableAreaRadius, playableAreaCenterPosition.x + playableAreaRadius);
		float z = Random.Range(playableAreaCenterPosition.z - playableAreaRadius, playableAreaCenterPosition.z + playableAreaRadius);
		Vector3 proposedPosition = new Vector3(x, 0, z);
		
		// Retry if it is too close to the player
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
	* Spawns a single dummy at the given position.
	*/
	public void SpawnDummy(Vector3 position)
	{
		position.y = defaultDummyY;
		
		// Turns the dummy towards the player (very spooky!!)
		Vector3 toPlayer = playerTransform.position - position;
		toPlayer.y = 0;
		
		Dummy dummy = Instantiate(dummyPrefab, position, Quaternion.LookRotation(toPlayer)).GetComponent<Dummy>();
		
		dummies.Add(dummy);
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