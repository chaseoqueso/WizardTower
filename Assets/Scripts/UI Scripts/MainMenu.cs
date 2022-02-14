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

        if(GameManager.instance){
            GameManager.instance.EnableJoining(false);
        }
    }

    public void PlayGame()
    {
        Debug.Log("Playing game!");
        // TODO: Load Scene
        SceneManager.LoadScene("CharacterSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting game!");
    }
}
