using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public Vector2 moveInput;
    public Vector2 lookInput;
    private Vector2 mousePrevious;

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
    }

    public void OnMoveLeft(InputValue input)
    {
        moveInput.x = -input.Get<float>();
    }

    public void OnMoveLeftCanceled()
    {
        moveInput.x = 0;
    }

    public void OnMoveRight(InputValue input)
    {
        moveInput.x = input.Get<float>();
    }    

    public void OnMoveRightCanceled()
    {
        moveInput.x = 0;
    }

    public void OnMoveForward(InputValue input)
    {
        moveInput.y = input.Get<float>();
    }    

    public void OnMoveForwardCanceled()
    {
        moveInput.y = 0;
    }

    public void OnMoveBack(InputValue input)
    {
        moveInput.y = -input.Get<float>();
    }

    public void OnMoveBackCanceled()
    {
        moveInput.y = 0;
    }

    public void OnShoot(InputValue input)
    {
        // TODO
    }

    // Picking up and dropping wizards
    public void OnWizardInteract(InputValue input)
    {
        // TODO (Chase)
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
        if(mousePrevious != Vector2.zero)
            lookInput = mouse - mousePrevious;
        mousePrevious = mouse;
        Debug.Log(lookInput);
    }

    public void OnPause(InputValue input)
    {
        // TODO (Jen)
    }
}
