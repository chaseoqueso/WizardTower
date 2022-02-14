using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public enum WizardType{
    redCone,
    greenDiamond,
    yellowRound,
    blueSquare,
    
    enumSize
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float timeSinceLevel;
    [HideInInspector] public float timeSinceStart;

    [SerializeField] private List<GameObject> modelPrefabs = new List<GameObject>();
    public Dictionary<WizardType, GameObject> wizardModelDatabase {get; private set;}

    public PlayerInputManager playerInputManager {get; private set;}
    public Dictionary<int,GameObject> playerDatabase {get; private set;}
    
    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        SetupModelDatabase();

        playerInputManager = GetComponent<PlayerInputManager>();
        playerDatabase = new Dictionary<int, GameObject>();

        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void SetupModelDatabase()
    {
        wizardModelDatabase = new Dictionary<WizardType, GameObject>();

        if( (int)WizardType.enumSize != modelPrefabs.Count ){
            Debug.LogError("Wizard type enum != model prefab options! Failed to setup model database.");
            return;
        }

        // Model prefabs in the list MUST be the same order as the enum values
        for(int i = 0; i < (int)WizardType.enumSize; i++){
            wizardModelDatabase[(WizardType)i] = modelPrefabs[i];
        }
    }

    public GameObject GetWizardModelFromType(WizardType type)
    {
        return wizardModelDatabase[type];
    }

    public void OnPlayerJoined()
    {
        Debug.Log("Player joined! Num players: " + playerInputManager.playerCount);
        GameManager.instance.EnableJoining(false);

        // Find all players in scene
        Player[] players = FindObjectsOfType<Player>();

        // Loop through to find the new player
        foreach(Player p in players){

            // If the player number has not yet been set, equals default 0 -> set new values
            if(p.playerNumber == 0){
                p.playerNumber = playerInputManager.playerCount;
                playerDatabase[p.playerNumber] = p.gameObject;

                // Set this player to a child of the Game Manager
                p.transform.parent = transform;

                CharSelectPanel panel = CharacterSelect.instance.GetPanelFromPlayerNum(p.playerNumber);
                panel.playerIsJoining = true;
                panel.ToggleJoinOverlay(false);
                panel.SetPlayerJoiningUI(true);

                break;
            }
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

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
            OnGameStart();
    }

    public void OnGameStart()
    {
        foreach(KeyValuePair<int, GameObject> entry in playerDatabase){
            GameObject player = entry.Value;
            player.transform.parent = null;
            SceneManager.MoveGameObjectToScene(player, SceneManager.GetActiveScene());
            player.GetComponent<Player>().Initialize(entry.Key);
        }
    }

    public void GameOver()
    {
        // Open game over UI

        Debug.Log("Game over!");

        playerDatabase.Clear();
    }
}
