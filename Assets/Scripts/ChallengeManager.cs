using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents the challenge game mode.
 * In this mode, the goal is to destroy as many dummy as possible in a limited time.
 */
public class ChallengeManager : MonoBehaviour
{
    public float maxTime;

    public float remainingTime;

    public int currentScore;

    public int scorePerDummy = 1;
    
    public DummySpawner spawner;

    public bool gameRunning = false;

    /**
     * Number of points gained before the number of targets increases.
     */
    public int difficultyIncreaseFrequency = 5;

    /**
     * Number of seconds added to the timer on destroying a dummy.
     */
    public int timeIncreaseOnDummyKill = 5;

    /**
     * The number of targets placed on a new dummy.
     */
    public int defaultTargetCount = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                EndGame();
            }
        }
    }

    public void StartGame()
    {
        Debug.Log("Challenge starting...");
        gameRunning = true;
        remainingTime = maxTime;
        spawner.running = true;
        currentScore = 0;
    }

    public void EndGame()
    {
        Debug.Log("Challenge ended. Score: " + currentScore);
        gameRunning = false;
        spawner.running = false;
        spawner.RemoveAllDummies();
    }

    public void DestroyDummy(Dummy dummy)
    {
        currentScore += scorePerDummy;
        remainingTime += 5;
        ManageDifficulty();
        spawner.RemoveDummy(dummy);
    }

    /**
     * Makes the game more difficult as time goes on.
     */
    public void ManageDifficulty()
    {
        if (currentScore % difficultyIncreaseFrequency == 0)
        {
            defaultTargetCount += 1;
        }
    }
}
