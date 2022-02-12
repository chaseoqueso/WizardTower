using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerPrefab;

    [HideInInspector] public float timeSinceStart;

    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void GameOver()
    {
        // Open game over UI

        Debug.Log("Game over!");
    }
}
