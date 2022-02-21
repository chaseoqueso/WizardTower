using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

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
    private float thisGameScoreValue;

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
            // OnGameOverSetPlayerOneDesignatedUIWizard();

            foreach(KeyValuePair<int, GameObject> entry in GameManager.instance.playerDatabase){
                Destroy(entry.Value);
            }
            GameManager.instance.playerDatabase.Clear();
        }
    }

    private void OnGameOverSetPlayerOneDesignatedUIWizard()
    {
        // Deactivate the inputs of all players except player 1
        GameObject playerOne = GameManager.instance.playerDatabase[1];
        foreach(GameObject player in GameManager.instance.playerDatabase.Values){
            if(player != playerOne){
                // Destroy()
            }
        }

        // Use PlayerInput.SwitchCurrentActionMap("Menu") on the character who paused the game
        playerOne.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

        // Assign the action map instance of the player who paused to the Input Sytem UI Input Module using: InputSystemUiInputModule.ActionsAsset = PlayerInput.actions
        FindObjectOfType<InputSystemUIInputModule>().actionsAsset = GetComponent<PlayerInput>().actions;
    }

    public void ReturnToMenu()
    {
        ToggleGameOverUI(false);

        // Undo the above input system stuff
        // foreach(GameObject player in GameManager.instance.playerDatabase.Values){
        //     player.GetComponent<PlayerInput>().enabled = true;
        // }
        // GameManager.instance.playerDatabase[1].GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");

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
                PlayerPrefs.SetFloat(((ScoreSlots)i).ToString(), thisGameScoreValue);
                PlayerPrefs.Save();
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

        // string minText = min + "";
        // if(min < 10){
        //     minText = "0" + minText;
        // }

        string secondsText = sec + "";
        if(sec < 10){
            secondsText = "0" + secondsText;
        }

        return min + ":" + secondsText;
    }

    private void ReplaceSlotValue(ScoreSlots slot, float newTime)
    {
        // Starting at that slot, shift everything down
        for( int i = (int)slot; i < (int)ScoreSlots.enumSize; i++ ){
            // Temp store the old value
            float valueToMoveDown = PlayerPrefs.GetFloat(((ScoreSlots)i).ToString());

            // Set this slot to the new value
            PlayerPrefs.SetFloat(((ScoreSlots)i).ToString(), newTime);
            PlayerPrefs.Save();

            // Set the next slot to the next value
            newTime = valueToMoveDown;            
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
