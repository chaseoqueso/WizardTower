using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class CharSelectPanel : MonoBehaviour
{
    [SerializeField] private int playerNum;

    [SerializeField] private Image wizardIcon;
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private TMP_Text playerNumText;
    [SerializeField] private GameObject joinOverlay;

    [HideInInspector] public bool playerIsJoining = false;

    public int PlayerNum()
    {
        return playerNum;
    }

    public void ToggleJoinOverlay(bool set)
    {
        joinOverlay.gameObject.SetActive(set);
    }

    public void PlayerReady(CharSelectWizardButton wizardButton)
    {
        ToggleReadyStatus(true);
        CharacterSelect.instance.readyPlayers.Add(playerNum);

        if(CharacterSelect.instance.readyPlayers.Count == 4){
            CharacterSelect.instance.CanStartGame(true);
        }

        // TODO: below is TEMP (will later be set in the SetPlayerModel function)
        wizardIcon.sprite = wizardButton.gameObject.GetComponent<Image>().sprite;    
        wizardIcon.SetNativeSize();

        SetPlayerModel(wizardButton.WizardType());

        SetPlayerJoiningUI(false);

        if(GameManager.instance.playerInputManager.playerCount < 4){
            GameManager.instance.EnableJoining(true);
        }
    }

    public void PlayerCanceled()
    {
        ToggleReadyStatus(false);
        CharacterSelect.instance.readyPlayers.Remove(playerNum);

        SetPlayerJoiningUI(true);

        CharacterSelect.instance.CanStartGame(false);
    }

    public void SetPlayerJoiningUI(bool set)
    {
        if(set){
            // TEMP
            wizardIcon.sprite = null;
            readyText.gameObject.SetActive(false);
            playerNumText.color = Color.yellow;
        }
        else{
            readyText.gameObject.SetActive(true);
            playerNumText.color = Color.white;
        }
    }

    private void ToggleReadyStatus(bool set)
    {
        readyText.gameObject.SetActive(set);
    }

    public void SetPlayerModel(WizardType type)
    {
        // Get the player
        GameObject player = GameManager.instance.playerDatabase[playerNum];
        player.GetComponent<Player>().wizardType = type;

        // Get the model prefab
        GameObject model = GameManager.instance.GetWizardModelFromType(type);

        // Instantiate the model as a child of the player
        Instantiate(model, player.transform);

        // TODO: Set the UI to reflect that character selection

    }
}
