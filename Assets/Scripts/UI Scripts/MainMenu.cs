using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.Select();
    }

    public void PlayGame()
    {
        Debug.Log("Playing game!");
        // TODO: Load Scene
        // SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game!");
    }
}
