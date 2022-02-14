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

    [HideInInspector] public static bool gameOverUIActive {get; private set;}

    [SerializeField] private TMP_Text thisGameScoreText;
    private float thisGameScoreValue = 0f;

    [SerializeField] private TMP_Text firstSlot;
    [SerializeField] private TMP_Text secondSlot;
    [SerializeField] private TMP_Text thirdSlot;
    [SerializeField] private TMP_Text fourthSlot;
    [SerializeField] private TMP_Text fifthSlot;

    [SerializeField] private Button menuButton;

    private const string NO_SCORE = "-";

    void Start()
    {
        gameOverUIActive = false;

        // If player prefs, get them and set the values in the UI
        if( PlayerPrefs.HasKey(ScoreSlots.First.ToString()) ){
            SetHighScoreTextFromPrefs();
            return;
        }

        // If no player prefs yet, set them up AND set the UI to have a bunch of dashes
        PlayerPrefs.SetFloat(ScoreSlots.First.ToString(), 0);
        PlayerPrefs.SetFloat(ScoreSlots.Second.ToString(), 0);
        PlayerPrefs.SetFloat(ScoreSlots.Third.ToString(), 0);
        PlayerPrefs.SetFloat(ScoreSlots.Fourth.ToString(), 0);
        PlayerPrefs.SetFloat(ScoreSlots.Fifth.ToString(), 0);
        PlayerPrefs.Save();
    }

    // Could pass in the time value here (with a default) and then also into SetScoreOnGameOver
    public void ToggleGameOverUI(bool set)
    {
        gameOverPanel.SetActive(set);
        gameOverUIActive = set;

        if(set){
            Time.timeScale = 0f;
            menuButton.Select();
            SetScoreOnGameOver();
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
        thisGameScoreValue = GameManager.instance.timeSinceLevel;
        thisGameScoreText.text = ConvertSecondsToReadableString(thisGameScoreValue);

        // === If necessary, adjust 5 high score ranks ===

        for(int i = 0; i < (int)ScoreSlots.enumSize; i++){
            // If there is no score for a slot, set it to the earliest of such slots
            if( PlayerPrefs.GetFloat( ((ScoreSlots)i).ToString() ) == 0f ){
                ReplaceSlotValue( (ScoreSlots)i, thisGameScoreValue );
                return;
            }

            // Otherwise, compare this time stamp to the existing ones; if this is bigger than one, move everything down
            float oldScore = PlayerPrefs.GetFloat(((ScoreSlots)i).ToString());
            if(oldScore < thisGameScoreValue){
                ReplaceSlotValue( (ScoreSlots)i, thisGameScoreValue );
                return;
            }
        }

        // Update the UI accordingly
        SetHighScoreTextFromPrefs();
    }

    private string ConvertSecondsToReadableString( float timeInSeconds )
    {
        if(timeInSeconds == 0){
            return NO_SCORE;
        }

        int min = (int)Mathf.Floor( timeInSeconds / 60 );
        int sec = (int)Mathf.Floor( timeInSeconds % 60 );

        return min + ":" + sec;
    }

    private void ReplaceSlotValue(ScoreSlots slot, float newTime)
    {
        // Starting at that slot, shift everything down
        for( int i = (int)slot; i < (int)ScoreSlots.enumSize; i++ ){
            // Temp store the old value
            float valueToMoveDown = PlayerPrefs.GetFloat(((ScoreSlots)i).ToString());

            // Set this slot to the new value
            PlayerPrefs.SetFloat(((ScoreSlots)i).ToString(), newTime);

            // Set the next slot to the next value
            newTime = valueToMoveDown;
            
            PlayerPrefs.Save();
        }
    }

    private void SetHighScoreTextFromPrefs()
    {
        for(int i = 0; i < (int)ScoreSlots.enumSize; i++){
            float scoreValue = PlayerPrefs.GetFloat(((ScoreSlots)i).ToString());
            string readableStringScore = ConvertSecondsToReadableString(scoreValue);
            
            // If THIS game's score, make it stand out more
            if( scoreValue == thisGameScoreValue ){
                readableStringScore = "<b><color=purple>" + readableStringScore + "</b></color>";
            }

            switch( (ScoreSlots)i ){
                case ScoreSlots.First:
                    firstSlot.text = readableStringScore;
                    break;
                case ScoreSlots.Second:
                    secondSlot.text = readableStringScore;
                    break;
                case ScoreSlots.Third:
                    thirdSlot.text = readableStringScore;
                    break;
                case ScoreSlots.Fourth:
                    fourthSlot.text = readableStringScore;
                    break;
                case ScoreSlots.Fifth:
                    fifthSlot.text = readableStringScore;
                    break;
            }
        }
    }
}
