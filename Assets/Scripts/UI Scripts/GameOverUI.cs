using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public enum ScoreSlots{
    First,
    Second,
    Third,
    Fourth,
    Fifth,
    enumSize
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

    [SerializeField] private Button menuButton;

    private const string NO_SCORE = "-";

    void Start()
    {
        // If player prefs, get them and set the values in the UI
        if( PlayerPrefs.HasKey(ScoreSlots.First.ToString()) ){
            SetHighScoreTextFromPrefs();
            return;
        }

        // If no player prefs yet, set them up AND set the UI to have a bunch of dashes
        PlayerPrefs.SetString(ScoreSlots.First.ToString(), NO_SCORE);
        PlayerPrefs.SetString(ScoreSlots.Second.ToString(), NO_SCORE);
        PlayerPrefs.SetString(ScoreSlots.Third.ToString(), NO_SCORE);
        PlayerPrefs.SetString(ScoreSlots.Fourth.ToString(), NO_SCORE);
        PlayerPrefs.SetString(ScoreSlots.Fifth.ToString(), NO_SCORE);
        PlayerPrefs.Save();
    }

    // Could pass in the time value here (with a default) and then also into SetScoreOnGameOver
    public void ToggleGameOverUI(bool set)
    {
        gameOverPanel.SetActive(set);

        if(set){
            menuButton.Select();
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

    public void SetScoreOnGameOver()
    {
        // TODO: Set this score value
        thisGameScore.text = NO_SCORE;

        // === If necessary, adjust 5 high score ranks ===

        for(int i = 0; i < (int)ScoreSlots.enumSize; i++){
            // If there is no score for a slot, set it to the earliest of such slots
            if( PlayerPrefs.GetString( ((ScoreSlots)i).ToString() ).Equals(NO_SCORE) ){
                // Replace with this one
                return;
            }

            // Otherwise, compare this time stamp to the existing ones
            // If this is bigger than one, move everything down

            string oldScore = PlayerPrefs.GetString(((ScoreSlots)i).ToString());

            // if( oldScore -> convert to # -> compare to time stamp ){
            //     ReplaceSlotValue( (ScoreSlots)i, newTime )
            //     return;
            // }
        }

        // Update the UI accordingly
        SetHighScoreTextFromPrefs();
    }

    private void ReplaceSlotValue(ScoreSlots slot, string newTime)
    {
        // Starting at that slot, shift everything down
        for( int i = (int)slot; i < (int)ScoreSlots.enumSize; i++ ){
            // Temp store the old value
            string valueToMoveDown = PlayerPrefs.GetString(((ScoreSlots)i).ToString());

            // Set this slot to the new value
            PlayerPrefs.SetString(((ScoreSlots)i).ToString(), newTime);

            // Set the next slot to the next value
            newTime = valueToMoveDown;
            
            PlayerPrefs.Save();
        }
    }

    private void SetHighScoreTextFromPrefs()
    {
        for(int i = 0; i < (int)ScoreSlots.enumSize; i++){

            string score = PlayerPrefs.GetString(((ScoreSlots)i).ToString());
            // If THIS game's score, make it stand out more
            if(score.Equals(thisGameScore.text)){
                score = "<b><color=blue>" + score + "</b></color>";
            }

            switch( (ScoreSlots)i ){
                case ScoreSlots.First:
                    firstSlot.text = score;
                    break;
                case ScoreSlots.Second:
                    secondSlot.text = score;
                    break;
                case ScoreSlots.Third:
                    thirdSlot.text = score;
                    break;
                case ScoreSlots.Fourth:
                    fourthSlot.text = score;
                    break;
                case ScoreSlots.Fifth:
                    fifthSlot.text = score;
                    break;
            }
        }
    }
}
