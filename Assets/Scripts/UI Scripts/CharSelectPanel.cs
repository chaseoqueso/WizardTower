using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class CharSelectPanel : MonoBehaviour
{
    [SerializeField] private int playerNum;

    [SerializeField] private Button submitButton;
    [SerializeField] private TMP_Text readyText;

    public int PlayerNum()
    {
        return playerNum;
    }

    public void PlayerReady()
    {
        ToggleReadyStatus(true);
        CharacterSelect.instance.readyPlayers.Add(playerNum);

        if(CharacterSelect.instance.readyPlayers.Count == 4){
            CharacterSelect.instance.CanStartGame(true);
        }

        SetNewPlayerValues();
    }

    public void PlayerCanceled()
    {
        ToggleReadyStatus(false);
        CharacterSelect.instance.readyPlayers.Remove(playerNum);

        CharacterSelect.instance.CanStartGame(false);
    }

    private void ToggleReadyStatus(bool set)
    {
        readyText.gameObject.SetActive(set);
        submitButton.gameObject.SetActive(!set);
    }

    public void OnSubmit(InputValue input)
    {
        PlayerReady();
    }

    public void OnCancel(InputValue input)
    {
        PlayerCanceled();
    }

    public void SetNewPlayerValues()
    {
        // Get this player object somehow

        // Set player #

        // Set the model
        
    }
}
