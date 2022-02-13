using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WizardGridUIAlert : MonoBehaviour
{
    public static WizardGridUIAlert instance;

    [SerializeField] private Image blueWizardBox;
    [SerializeField] private Image yellowWizardBox;
    [SerializeField] private Image redWizardBox;
    [SerializeField] private Image greenWizardBox;

    [SerializeField] private GameObject blueWizardBlank;
    [SerializeField] private GameObject yellowWizardBlank;
    [SerializeField] private GameObject redWizardBlank;
    [SerializeField] private GameObject greenWizardBlank;

    [SerializeField] private TMP_Text blueWizardPlayerNum;
    [SerializeField] private TMP_Text yellowWizardPlayerNum;
    [SerializeField] private TMP_Text redWizardPlayerNum;
    [SerializeField] private TMP_Text greenWizardPlayerNum;

    void Awake()
    {
        if( instance ){
            Destroy(gameObject);
        }
        else{
            instance = this;
        }
    }

    void Start()
    {
        // Disable all borders -> enable them when a player joins
        ToggleBorderActive(false, WizardType.blueSquare);
        ToggleBorderActive(false, WizardType.greenDiamond);
        ToggleBorderActive(false, WizardType.redCone);
        ToggleBorderActive(false, WizardType.yellowRound);
    }

    public void ToggleBorderActive(bool set, WizardType type, string playerNum="")
    {
        switch(type){
            case WizardType.blueSquare:
                blueWizardBox.gameObject.SetActive(set);
                blueWizardPlayerNum.text = playerNum;
                blueWizardBlank.SetActive(!set);
                break;
            
            case WizardType.greenDiamond:
                greenWizardBox.gameObject.SetActive(set);
                greenWizardPlayerNum.text = playerNum;
                greenWizardBlank.SetActive(!set);
                break;
            
            case WizardType.redCone:
                redWizardBox.gameObject.SetActive(set);
                redWizardPlayerNum.text = playerNum;
                redWizardBlank.SetActive(!set);
                break;
            
            case WizardType.yellowRound:
                yellowWizardBox.gameObject.SetActive(set);
                yellowWizardPlayerNum.text = playerNum;
                yellowWizardBlank.SetActive(!set);
                break;
            
            default:
                Debug.LogWarning("Could not toggle UI border active for wizard type: " + type.ToString());
                break;
        }
    }
}
