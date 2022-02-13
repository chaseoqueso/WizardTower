using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectWizardButton : MonoBehaviour
{
    [SerializeField] private WizardType wizardType;
    [SerializeField] private Button button;

    public WizardType WizardType()
    {
        return wizardType;
    }

    public void ToggleButtonInteractability(bool set)
    {
        button.interactable = set;
    }
}
