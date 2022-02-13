using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public enum WizardType{
    yellowRound,
    redCone,
    blueSquare,
    greenDiamond,

    enumSize
}

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

        // TODO: Get the type from the button!!! (below is temp)
        WizardType type = WizardType.blueSquare;
        SetPlayerModel(type);
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

    public void SetPlayerModel(WizardType type)
    {
        // Get the player
        GameObject player = GameManager.instance.playerDatabase[playerNum];

        // Get the model prefab
        GameObject model = GameManager.instance.GetWizardModelFromType(type);

        // Instantiate the model as a child of the player
        Instantiate(model, player.transform);
    }
}
