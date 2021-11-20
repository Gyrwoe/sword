using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text score;

    public Text time;

    public Text message;

    public ChallengeManager manager;

    private bool _gameWasRunning;
    
    // Start is called before the first frame update
    void Start()
    {
        message.text = "";
        score.text = "";
        time.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.gameRunning)
        {
            if (!_gameWasRunning)
            {
                _gameWasRunning = true;
            }
            UpdateUI();
        } else if (_gameWasRunning)
        {
            HideUI();
            _gameWasRunning = false;
            StartCoroutine(DisplayGameEnded());
        }
    }

    public void HideUI()
    {
        score.text = "";
        time.text = "";
    }
    public void UpdateUI()
    {
        score.text = "Score: " + manager.currentScore;
        time.text = "Time: " + (int)manager.remainingTime;
    }

    IEnumerator DisplayGameEnded()
    {
        message.text = "Game ended. Score: " + manager.currentScore;

        yield return new WaitForSeconds(5);
        
        message.text = "";
    }
}
