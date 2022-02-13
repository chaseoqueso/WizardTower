using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum ScoreSlots{
    First,
    Second,
    Third,
    Fourth,
    Fifth
}

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;

    [SerializeField] private TMP_Text thisGameScore;

    [SerializeField] private TMP_Text firstSlot;
    [SerializeField] private TMP_Text secondSlot;
    [SerializeField] private TMP_Text thirdSlot;
    [SerializeField] private TMP_Text fourthSlot;
    [SerializeField] private TMP_Text fifthSlot;

    void Start()
    {
        // If player prefs, get them and set the values in the UI
        // SetHighScoresFromPrefs();

        // If no player prefs yet, set them up AND set the UI to have a bunch of dashes
        // PlayerPrefs.SetString(ScoreSlots.First.ToString(), "");
        // PlayerPrefs.SetString(ScoreSlots.Second.ToString(), "");
        // PlayerPrefs.SetString(ScoreSlots.Third.ToString(), "");
        // PlayerPrefs.SetString(ScoreSlots.Fourth.ToString(), "");
        // PlayerPrefs.SetString(ScoreSlots.Fifth.ToString(), "");
        // PlayerPrefs.Save();
    }

    public void ToggleGameOverUI(bool set)
    {
        gameOverPanel.SetActive(set);

        if(set){
            SetScoreOnGameOver();
            Time.timeScale = 0f;
        }
    }

    public void ReturnToMenu()
    {
        ToggleGameOverUI(false);
        Time.timeScale = 1f;
        Debug.Log("Returning to Main Menu!");
        SceneManager.LoadScene("MainMenu");
    }

    private void SetScoreOnGameOver()
    {
        // Set this score value
        // thisGameScore.text = "";

        // === If necessary, adjust 5 high score ranks ===

        // if( score > any saved (or if < 5 are saved),
        // put it in the proper slot then save player prefs )
        // Also make THIS score a different color so that it stands out!


        // PlayerPrefs.Save();

        // Update the UI accordingly
        // highScores.text = "";
    }

    private void SetHighScoresFromPrefs()
    {
        // TODO
    }
}
