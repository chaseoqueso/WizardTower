using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class InputManager : MonoBehaviour
{
    public bool wizardInteract { get; private set; }
    public bool shoot { get; private set; }
    public Vector2 moveInput;
    public Vector2 lookInput;
    private Vector2 mouseLook;
    private Vector2 controllerLook;
    private Vector2 mousePrevious;
    private Vector2 lookPrevious;

    public bool inCharSelect;
    public bool playerIsJoining = true;

    public List<GameObject> wizardButtons = new List<GameObject>();
    public CharSelectWizardButton playerSelectedWizard = null;

    void Start()
    {
        InputActionAsset controls = GetComponent<PlayerInput>().actions;

        // Add STOPPING when you're no longer holding the button
        controls.FindAction("MoveLeft").canceled += x => OnMoveLeftCanceled();
        controls.FindAction("MoveRight").canceled += x => OnMoveRightCanceled();
        controls.FindAction("MoveForward").canceled += x => OnMoveForwardCanceled();
        controls.FindAction("MoveBack").canceled += x => OnMoveBackCanceled();

        GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();

        if(CharacterSelect.instance){
            inCharSelect = true;
            CharacterSelect.instance.wizardButtons[0].Select();

            foreach(Button b in CharacterSelect.instance.wizardButtons){
                wizardButtons.Add(b.gameObject);
            }

            SelectNextInteractableIcon();
        }      
    }

    void Update()
    {
        lookInput = mouseLook;
        if(lookInput == Vector2.zero)
            lookInput = controllerLook;

    }

    private void LateUpdate()
    {
        if(inCharSelect){
            return;
        }

        mouseLook = Vector2.zero;
        wizardInteract = false;
        shoot = false;
    }

    public void OnMoveLeft(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        moveInput.x = -input.Get<float>();
    }

    public void OnMoveLeftCanceled()
    {
        if(inCharSelect){
            return;
        }

        if(moveInput.x < 0)
            moveInput.x = 0;
    }

    public void OnMoveRight(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        moveInput.x = input.Get<float>();
    }    

    public void OnMoveRightCanceled()
    {
        if(inCharSelect){
            return;
        }

        if (moveInput.x > 0)
            moveInput.x = 0;
    }

    public void OnMoveForward(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        moveInput.y = input.Get<float>();
    }    

    public void OnMoveForwardCanceled()
    {
        if(inCharSelect){
            return;
        }

        if (moveInput.y > 0)
            moveInput.y = 0;
    }

    public void OnMoveBack(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        moveInput.y = -input.Get<float>();
    }

    public void OnMoveBackCanceled()
    {
        if(inCharSelect){
            return;
        }

        if (moveInput.y < 0)
            moveInput.y = 0;
    }

    public void OnShoot(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        shoot = input.Get<float>() == 1;
    }

    // Picking up and dropping wizards
    public void OnWizardInteract(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        wizardInteract = input.Get<float>() == 1;
    }

    // LOOKING on gamepad control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookGamepad(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        controllerLook = input.Get<Vector2>();
    }

    // LOOKING (both up and down) if you are on the keyboard control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookMouse(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        Vector2 mouse = input.Get<Vector2>();
        if (mousePrevious != Vector2.zero)
        {
            mouseLook = (mouse + mousePrevious)/2;
        }
        mousePrevious = mouse;
        lookPrevious = mouse - mousePrevious;
    }

    public void OnPause(InputValue input)
    {
        if(inCharSelect){
            return;
        }

        if(!PauseMenu.instance.gameIsPaused){
            PauseMenu.instance.PauseGame();
        }
        else{
            PauseMenu.instance.ResumeGame();
        }
    }

    // UI Input Stuff
    public void OnNavigate(InputValue input)
    {
        Debug.Log("Player " + GetComponent<Player>().playerNumber);

        // If we're on the character select screen
        if(inCharSelect && playerIsJoining){
            // If the currently selected button is a wizard button
            GameObject currentSelectedButton = EventSystem.current.currentSelectedGameObject;
            if(wizardButtons.Contains(currentSelectedButton)){

                // If a thing is currently selected, remove the alert
                if(playerSelectedWizard != null){
                    WizardGridUIAlert.instance.ToggleBorderActive(false, playerSelectedWizard.WizardType(), GetComponent<Player>().playerNumber.ToString());
                }

                playerSelectedWizard = currentSelectedButton.GetComponent<CharSelectWizardButton>();
                WizardGridUIAlert.instance.ToggleBorderActive(true, playerSelectedWizard.WizardType(), GetComponent<Player>().playerNumber.ToString());

                // If no other players have this selected, select it
                // if(!WizardIsAlreadySelectedByAnotherPlayer(currentSelectedButton.GetComponent<CharSelectWizardButton>())){
                //     playerSelectedWizard = currentSelectedButton.GetComponent<CharSelectWizardButton>();
                //     WizardGridUIAlert.instance.ToggleBorderActive(true, playerSelectedWizard.WizardType(), GetComponent<Player>().playerNumber.ToString());
                // }
                // else{   // If someone else has it selected, move on to the next one
                //     SelectNextInteractableIcon();
                // }

                Debug.Log("Player " + GetComponent<Player>().playerNumber + " Wizard Type: " + playerSelectedWizard.WizardType());
            }
        }
    }

    // If you click a submit button to join, it automatically picks that first character for you
    public void OnSubmit(InputValue input)
    {
        if( inCharSelect && playerSelectedWizard && playerIsJoining ){
            CharacterSelect.instance.PlayerReady(gameObject.GetComponent<Player>().playerNumber, playerSelectedWizard);
            WizardGridUIAlert.instance.ToggleBorderActive(false, playerSelectedWizard.WizardType(), GetComponent<Player>().playerNumber.ToString());
            playerIsJoining = false;
        }
    }

    // No longer working
    public void OnCancel(InputValue input)
    {
        if(inCharSelect && playerSelectedWizard){
            CharacterSelect.instance.PlayerCanceled(gameObject.GetComponent<Player>().playerNumber, playerSelectedWizard);
        }
        if(playerIsJoining && playerSelectedWizard == null){
            SelectNextInteractableIcon();
        }
    }

    public void SelectNextInteractableIcon()
    {
        Button b = CharacterSelect.instance.GetNextInteractableWizardIcon();
        b.Select();
        playerSelectedWizard = b.GetComponent<CharSelectWizardButton>();
        WizardGridUIAlert.instance.ToggleBorderActive(true, playerSelectedWizard.WizardType(), GetComponent<Player>().playerNumber.ToString());
    }

    // Not working
    public bool WizardIsAlreadySelectedByAnotherPlayer(CharSelectWizardButton wizardButton)
    {
        foreach(InputManager playerInput in FindObjectsOfType<InputManager>()){
            if( playerInput.playerSelectedWizard != null && playerInput.playerSelectedWizard == wizardButton ){
                return true;
            }
        }
        return false;
    }
}
