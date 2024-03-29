using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

/*
    UI COLORS:

    menu background lightest: #f6e7ff

    med: #6a3b8a

    dark: #4d395a


    === Colors from Buttons ===

    Light Purple: #e6bbff

    Darker Purple: #c377f0
*/

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect instance;

    public HashSet<int> readyPlayers = new HashSet<int>();
    [SerializeField] private List<CharSelectPanel> playerPanels = new List<CharSelectPanel>();

    public List<Button> wizardButtons = new List<Button>();
    
    [SerializeField] private Button startButton;

    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }

    void Start()
    {
        GameManager.instance.EnableJoining(true);
    }

    // Updated when the number of ready players updates
    public void CanStartGame(bool set)
    {
        if(set && (GameManager.instance.playerInputManager.playerCount != 4 || readyPlayers.Count != 4)){
            Debug.LogError("Cannot set start button interactable!\nNum Players: " + GameManager.instance.playerInputManager.playerCount + "\nNum Ready Players:" + readyPlayers.Count);
            return;
        }

        startButton.interactable = set;

        if(set){
            startButton.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 255);
            startButton.Select();
        }
        else{
            startButton.GetComponentInChildren<TMP_Text>().color = new Color32(255, 255, 255, 75);
        }
    }

    // Called when you press CONTINUE
    public void StartGame()
    {
        Debug.Log("Starting game!");
        SceneManager.LoadScene("GameScene");
        AudioManager.instance.Stop("TitleSong");
        AudioManager.instance.Play("GameplaySong");
    }

    public void BackButton()
    {
        Debug.Log("Returning to Main Menu!");
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayerReady(int playerNum, CharSelectWizardButton wizardButton)
    {
        wizardButton.ToggleButtonInteractability(false);

        foreach(CharSelectPanel panel in playerPanels){
            if(panel.PlayerNum() == playerNum){
                panel.PlayerReady(wizardButton);
            }
        }
    }

    public void PlayerCanceled(int playerNum, CharSelectWizardButton wizardButton)
    {
        foreach(CharSelectPanel panel in playerPanels){
            // If any player is currently joining, don't let a player cancel their selection
            if(panel.playerIsJoining){
                return;
            }
        }

        GameManager.instance.playerDatabase[playerNum].GetComponent<InputManager>().playerIsJoining = true;

        wizardButton.ToggleButtonInteractability(true);

        foreach(CharSelectPanel panel in playerPanels){
            if(panel.PlayerNum() == playerNum){
                panel.PlayerCanceled();
            }
        }
    }

    public CharSelectPanel GetPanelFromPlayerNum(int num)
    {
        foreach(CharSelectPanel panel in playerPanels){
            if(panel.PlayerNum() == num){
                return panel;
            }
        }
        return null;
    }

    public Button GetNextInteractableWizardIcon()
    {
        foreach(Button b in wizardButtons){
            if(b.interactable){
                return b;
            }
        }
        return null;
    }
}
