using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [HideInInspector] public float timeSinceStart;

    public Dictionary<int,GameObject> players {get; private set;}

    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        players = new Dictionary<int, GameObject>();
    }

    

    public void GameOver()
    {
        // Open game over UI

        Debug.Log("Game over!");

        players.Clear();
    }
}
