using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    }

    // Called when you press CONTINUE
    public void StartGame()
    {
        Debug.Log("Starting game!");
        // CHASE UNCOMMENT THIS THIS (add the scene to the build settings and then add the string name here)
        // SceneManager.LoadScene("");

        GameManager.instance.OnGameStart();
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
        wizardButton.ToggleButtonInteractability(true);

        foreach(CharSelectPanel panel in playerPanels){
            if(panel.PlayerNum() == playerNum){
                panel.PlayerCanceled();
            }
        }
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
