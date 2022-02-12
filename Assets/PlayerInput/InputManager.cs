using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    void Start()
    {
        InputActionAsset controls = GetComponent<PlayerInput>().actions;


        // Add STOPPING when you're no longer holding the button
        controls.FindAction("MoveLeft").canceled += x => OnMoveLeftCanceled();
        controls.FindAction("MoveRight").canceled += x => OnMoveRightCanceled();
        controls.FindAction("MoveForward").canceled += x => OnMoveForwardCanceled();
        controls.FindAction("MoveBack").canceled += x => OnMoveBackCanceled();
    }

    public void OnMoveLeft(InputValue input)
    {
        // TODO (Chase)
    }

    public void OnMoveLeftCanceled()
    {
        // TODO (Chase)
    }

    public void OnMoveRight(InputValue input)
    {
        // TODO (Chase)
    }    

    public void OnMoveRightCanceled()
    {
        // TODO (Chase)
    }

    public void OnMoveForward(InputValue input)
    {
        // TODO (Chase)
    }    

    public void OnMoveForwardCanceled()
    {
        // TODO (Chase)
    }

    public void OnMoveBack(InputValue input)
    {
        // TODO (Chase)
    }

    public void OnMoveBackCanceled()
    {
        // TODO (Chase)
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

    // LOOKING (only up) on gamepad control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookUpGamepad(InputValue input)
    {
        // TODO (Chase)
    }

    // LOOKING (only down) on gamepad control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookDownGamepad(InputValue input)
    {
        // TODO (Chase)
    }

    // LOOKING (both up and down) if you are on the keyboard control scheme
    // (Correct version should be automatically called based on your input device)
    public void OnLookMouse(InputValue input)
    {
        // TODO (Chase)
    }

    public void OnPause(InputValue input)
    {
        // TODO (Jen)
    }
}
