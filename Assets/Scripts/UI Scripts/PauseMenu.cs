using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    public bool gameIsPaused {get; private set;}

    [SerializeField] private GameObject pauseMenuUI;

    [SerializeField] private GameObject pauseMenuDefaultPanel;
    [SerializeField] private GameObject settingsMenuPanel;

    [SerializeField] private Button continueButton;

    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
        gameIsPaused = false;
    }

    public void ResumeGame()
    {
        ResetPauseUI();
        Time.timeScale = 1f;
        gameIsPaused = false;

        foreach(GameObject player in GameManager.instance.playerDatabase.Values){
            PlayerInput input = player.GetComponent<PlayerInput>();
            input.SwitchCurrentActionMap("Gameplay");
            input.ActivateInput();
        }
    }

    private void ResetPauseUI()
    {
        settingsMenuPanel.SetActive(false);
        // controlsMenuPanel.SetActive(false);
        pauseMenuDefaultPanel.SetActive(true);

        pauseMenuUI.SetActive(false);
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        continueButton.Select();
        gameIsPaused = true;
    }

    public void LoadMenu()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        Debug.Log("Returning to Main Menu!");
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleSettingsMenu(bool set)
    {
        settingsMenuPanel.SetActive(set);
        pauseMenuDefaultPanel.SetActive(!set);
    }

    // public void ToggleControlsMenu(bool set)
    // {
    //     controlsMenuPanel.SetActive(set);
    //     pauseMenuDefaultPanel.SetActive(!set);
    // }
}
