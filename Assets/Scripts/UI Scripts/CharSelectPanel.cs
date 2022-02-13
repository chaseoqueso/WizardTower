using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public enum WizardType{
    redCone,
    greenDiamond,
    yellowRound,
    blueSquare,
    
    enumSize
}

public class CharSelectPanel : MonoBehaviour
{
    [SerializeField] private int playerNum;

    [SerializeField] private Image wizardIcon;
    [SerializeField] private TMP_Text readyText;

    public int PlayerNum()
    {
        return playerNum;
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
    }

    public void PlayerCanceled()
    {
        ToggleReadyStatus(false);
        CharacterSelect.instance.readyPlayers.Remove(playerNum);

        // TEMP
        wizardIcon.sprite = null;

        CharacterSelect.instance.CanStartGame(false);
    }

    private void ToggleReadyStatus(bool set)
    {
        readyText.gameObject.SetActive(set);
    }

    public void SetPlayerModel(WizardType type)
    {
        // Get the player
        GameObject player = GameManager.instance.playerDatabase[playerNum];

        // Get the model prefab
        GameObject model = GameManager.instance.GetWizardModelFromType(type);

        // Instantiate the model as a child of the player
        Instantiate(model, player.transform);

        // TODO: Set the UI to reflect that character selection

    }
}
