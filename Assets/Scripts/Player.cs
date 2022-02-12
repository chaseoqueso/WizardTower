using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float moveSpeed = 5;

    public bool enableMovement;
    public bool enableLook;
    public float xLookSensitivity = 5;
    public float yLookSensitivity = 5;

    private InputManager input;
    private CharacterController controller;
    private new Camera camera;
    private Vector3 moveInput;
    private Vector2 lookInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        input = GetComponent<InputManager>();
    }

    void Update()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        if(enableLook)
        {
            transform.Rotate(Vector3.up, lookInput.x * xLookSensitivity);

            camera.transform.Rotate(Vector3.right, -lookInput.y * yLookSensitivity);

            if(Vector3.Dot(camera.transform.forward, transform.forward) < 0)
            {
                if(lookInput.y < 0)
                    camera.transform.rotation = Quaternion.LookRotation(-Vector3.up, transform.forward);
                if(lookInput.y > 0)
                    camera.transform.rotation = Quaternion.LookRotation(Vector3.up, -transform.forward);
            }
        }
        moveInput = new Vector3(input.moveInput.x, 0, input.moveInput.y);
    }

    void FixedUpdate()
    {
        if(enableMovement)
        {
            controller.Move(Quaternion.LookRotation(transform.forward) * moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
