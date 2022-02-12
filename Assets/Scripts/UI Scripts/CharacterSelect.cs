using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class CharacterSelect : MonoBehaviour
{
    public static CharacterSelect instance;

    private PlayerInputManager playerInputManager;
    public HashSet<int> readyPlayers = new HashSet<int>();

    [SerializeField] private Button startButton;

    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }

        playerInputManager = GetComponent<PlayerInputManager>();
    }

    void Start()
    {
        EnableJoining(true);
    }

    // When a player joins, increase player number
    // This might not be the right name for this function...
    public void OnPlayerJoined()
    {
        Debug.Log("Player joined! Num players: " + playerInputManager.playerCount);

        // TODO: Move the new player object to be a child of the GameManager

        if(playerInputManager.playerCount == 4){
            EnableJoining(false);
        }
    }

    // Make sure this doesn't cause problems with CharSelectPanel PlayerCanceled()
    public void OnPlayerLeft()
    {
        Debug.Log("Lost a player! Num players: " + playerInputManager.playerCount);

        if(playerInputManager.playerCount < 4){
            EnableJoining(true);
            CharacterSelect.instance.CanStartGame(false);
        }
    }

    public void EnableJoining(bool set)
    {
        if(set){
            playerInputManager.EnableJoining();
        }
        else{
            playerInputManager.DisableJoining();
        }
    }

    // Updated when the number of ready players updates
    public void CanStartGame(bool set)
    {
        if(set && (playerInputManager.playerCount != 4 || readyPlayers.Count != 4)){
            Debug.LogError("Cannot set start button interactable!\nNum Players: " + playerInputManager.playerCount + "\nNum Ready Players:" + readyPlayers.Count);
            return;
        }

        startButton.interactable = set;
    }

    // Called when you press START
    public void StartGame()
    {
        Debug.Log("Starting game!");
        // TODO: SceneManager.LoadScene("");

        // TODO: Once in the new scene, move Player objects to no longer be children of the Game Manager
    }
}
