using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public bool wizardInteract { get; private set; }
    public bool shoot { get; private set; }
    public Vector2 moveInput;
    public Vector2 lookInput;
    private Vector2 mousePrevious;
    private Vector2 lookPrevious;

    void Start()
    {
        InputActionAsset controls = GetComponent<PlayerInput>().actions;

        // Add STOPPING when you're no longer holding the button
        controls.FindAction("MoveLeft").canceled += x => OnMoveLeftCanceled();
        controls.FindAction("MoveRight").canceled += x => OnMoveRightCanceled();
        controls.FindAction("MoveForward").canceled += x => OnMoveForwardCanceled();
        controls.FindAction("MoveBack").canceled += x => OnMoveBackCanceled();
    }

    private void LateUpdate()
    {
        lookInput = Vector2.zero;
        wizardInteract = false;
        shoot = false;
    }

    public void OnMoveLeft(InputValue input)
    {
        moveInput.x = -input.Get<float>();
    }

    public void OnMoveLeftCanceled()
    {
        if(moveInput.x < 0)
            moveInput.x = 0;
    }

    public void OnMoveRight(InputValue input)
    {
        moveInput.x = input.Get<float>();
    }    

    public void OnMoveRightCanceled()
    {
        if (moveInput.x > 0)
            moveInput.x = 0;
    }

    public void OnMoveForward(InputValue input)
    {
        moveInput.y = input.Get<float>();
    }    

    public void OnMoveForwardCanceled()
    {
        if (moveInput.y > 0)
            moveInput.y = 0;
    }

    public void OnMoveBack(InputValue input)
    {
        moveInput.y = -input.Get<float>();
    }

    public void OnMoveBackCanceled()
    {
        if (moveInput.y < 0)
            moveInput.y = 0;
    }

    public void OnShoot(InputValue input)
    {
        shoot = input.Get<float>() == 1;
    }

    // Picking up and dropping wizards
    public void OnWizardInteract(InputValue input)
    {
        wizardInteract = input.Get<float>() == 1;
    }

    // LOOKING on gamepad control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookGamepad(InputValue input)
    {
        lookInput = input.Get<Vector2>();
    }

    // LOOKING (both up and down) if you are on the keyboard control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookMouse(InputValue input)
    {
        Vector2 mouse = input.Get<Vector2>();
        if (mousePrevious != Vector2.zero)
        {
            lookInput = ((mouse - mousePrevious) + lookPrevious)/2;
        }
        mousePrevious = mouse;
        lookPrevious = mouse - mousePrevious;
    }

    public void OnPause(InputValue input)
    {
        // TODO (Jen)
    }

    // UI Input Stuff
    public void OnSubmit(InputValue input)
    {
        if( CharacterSelect.instance ){
            CharacterSelect.instance.PlayerReady(gameObject.GetComponent<Player>().playerNumber);
        }
    }

    public void OnCancel(InputValue input)
    {
        if(CharacterSelect.instance){
            CharacterSelect.instance.PlayerCanceled(gameObject.GetComponent<Player>().playerNumber);
        }
    }
}
