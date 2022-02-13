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

    public List<GameObject> wizardButtons = new List<GameObject>();
    private CharSelectWizardButton playerSelectedWizard = null;

    void Start()
    {
        InputActionAsset controls = GetComponent<PlayerInput>().actions;

        // Add STOPPING when you're no longer holding the button
        controls.FindAction("MoveLeft").canceled += x => OnMoveLeftCanceled();
        controls.FindAction("MoveRight").canceled += x => OnMoveRightCanceled();
        controls.FindAction("MoveForward").canceled += x => OnMoveForwardCanceled();
        controls.FindAction("MoveBack").canceled += x => OnMoveBackCanceled();

        if(CharacterSelect.instance){
            inCharSelect = true;
            CharacterSelect.instance.wizardButtons[0].Select();

            foreach(Button b in CharacterSelect.instance.wizardButtons){
                wizardButtons.Add(b.gameObject);
            }
        }

        GetComponent<PlayerInput>().uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
    }

    void Update()
    {
        // If we're on the character select screen
        if(inCharSelect){
            if(wizardButtons.Contains(EventSystem.current.currentSelectedGameObject)){
                playerSelectedWizard = EventSystem.current.currentSelectedGameObject.GetComponent<CharSelectWizardButton>();
            }
        }

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
            mouseLook = ((mouse - mousePrevious) + lookPrevious)/2;
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
    public void OnSubmit(InputValue input)
    {
        // if we're on the character select screen
        // and THIS PLAYER has a button selected (how do we set THIS value tho???)
        // and they CLICK
        // THEN call Wizard Selected, giving it the info about THIS CHARACTER who selected the wizard

        if( inCharSelect && playerSelectedWizard ){
            CharacterSelect.instance.PlayerReady(gameObject.GetComponent<Player>().playerNumber, playerSelectedWizard);
        }
    }

    public void OnCancel(InputValue input)
    {
        if(inCharSelect && playerSelectedWizard){
            CharacterSelect.instance.PlayerCanceled(gameObject.GetComponent<Player>().playerNumber, playerSelectedWizard);
        }
        playerSelectedWizard = null;

        foreach(Button b in CharacterSelect.instance.wizardButtons){
            if(b.interactable){
                b.Select();
                return;
            }
        }
    }
}

// TODO: Set inCharSelect = false when you start the game or return to main menu
