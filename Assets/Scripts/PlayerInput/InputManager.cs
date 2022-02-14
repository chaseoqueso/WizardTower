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
        // Does this need a can accept input check? 

        lookInput = mouseLook;
        if(lookInput == Vector2.zero)
            lookInput = controllerLook;

    }

    private bool CanAcceptInput()
    {
        if(inCharSelect || GameOverUI.gameOverUIActive){
            return false;
        }
        return true;
    }

    private void LateUpdate()
    {
        if(!CanAcceptInput()){
            return;
        }

        mouseLook = Vector2.zero;
        wizardInteract = false;
        shoot = false;
    }

    public void OnMoveLeft(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        moveInput.x = -input.Get<float>();
    }

    public void OnMoveLeftCanceled()
    {
        if(!CanAcceptInput()){
            return;
        }

        if(moveInput.x < 0)
            moveInput.x = 0;
    }

    public void OnMoveRight(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        moveInput.x = input.Get<float>();
    }    

    public void OnMoveRightCanceled()
    {
        if(!CanAcceptInput()){
            return;
        }

        if (moveInput.x > 0)
            moveInput.x = 0;
    }

    public void OnMoveForward(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        moveInput.y = input.Get<float>();
    }    

    public void OnMoveForwardCanceled()
    {
        if(!CanAcceptInput()){
            return;
        }

        if (moveInput.y > 0)
            moveInput.y = 0;
    }

    public void OnMoveBack(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        moveInput.y = -input.Get<float>();
    }

    public void OnMoveBackCanceled()
    {
        if(!CanAcceptInput()){
            return;
        }

        if (moveInput.y < 0)
            moveInput.y = 0;
    }

    public void OnShoot(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        shoot = input.Get<float>() == 1;
    }

    // Picking up and dropping wizards
    public void OnWizardInteract(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        wizardInteract = input.Get<float>() == 1;
    }

    // LOOKING on gamepad control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookGamepad(InputValue input)
    {
        if(!CanAcceptInput()){
            return;
        }

        controllerLook = input.Get<Vector2>();
    }

    // LOOKING (both up and down) if you are on the keyboard control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookMouse(InputValue input)
    {
        if(!CanAcceptInput()){
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
        if(!CanAcceptInput()){
            return;
        }

        if(!PauseMenu.instance.gameIsPaused){
            PauseMenu.instance.PauseGame();

            // Deactivate the inputs of all players except this one
            foreach(GameObject player in GameManager.instance.playerDatabase.Values){
                if(player != gameObject){
                    player.GetComponent<PlayerInput>().enabled = false;
                }
            }

            // Use PlayerInput.SwitchCurrentActionMap("Menu") on the character who paused the game
            GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");

            // Assign the action map instance of the player who paused to the Input Sytem UI Input Module using: InputSystemUiInputModule.ActionsAsset = PlayerInput.actions
            FindObjectOfType<InputSystemUIInputModule>().actionsAsset = GetComponent<PlayerInput>().actions;
        }
        else{
            PauseMenu.instance.ResumeGame();
            // Undo the above stuff
            foreach(GameObject player in GameManager.instance.playerDatabase.Values){
                player.GetComponent<PlayerInput>().enabled = true;
            }
            GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
        }
    }

    // UI Input Stuff
    public void OnNavigate(InputValue input)
    {
        // TODO: Uncomment once audio manager exists
        // AudioManager.instance.Play("ClickSound");

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
