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
    [SerializeField] List<CharSelectPanel> playerPanels = new List<CharSelectPanel>();

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
        // TODO: SceneManager.LoadScene("");

        // TODO: Once in the new scene, move Player objects to no longer be children of the Game Manager
    }

    public void PlayerReady(int playerNum)
    {
        foreach(CharSelectPanel panel in playerPanels){
            if(panel.PlayerNum() == playerNum){
                panel.PlayerReady();
            }
        }
    }

    public void PlayerCanceled(int playerNum)
    {
        foreach(CharSelectPanel panel in playerPanels){
            if(panel.PlayerNum() == playerNum){
                panel.PlayerCanceled();
            }
        }
    }
}
