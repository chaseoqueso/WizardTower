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

    private Coroutine liftRoutine;
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
        moveInput = new Vector3(input.moveInput.x, 0, input.moveInput.y);
        lookInput = input.lookInput;

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

        if (input.wizardInteract && liftRoutine == null)
        {
            Debug.Log("StartRoutine");
            RaycastHit hit;
            Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 5, Color.yellow, 1);
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 5, LayerMask.GetMask("Player")))
            {
                Debug.Log("HitPlayer");
                liftRoutine = StartCoroutine(PickupRoutine(hit.transform));
            }
        }
    }

    void FixedUpdate()
    {
        if(enableMovement)
        {
            controller.Move(Quaternion.LookRotation(transform.forward) * moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private IEnumerator PickupRoutine(Transform otherPlayer)
    {
        float pickupProgress = 0;
        while(pickupProgress < 1)
        {
            float angle = Mathf.Lerp(0, Mathf.PI / 2, pickupProgress);
            Vector3 offset = transform.rotation * new Vector3(0, Mathf.Sin(angle), Mathf.Cos(angle)) * 2;

            otherPlayer.position = transform.position + offset;
            pickupProgress += 2f * Time.deltaTime;

            yield return null;
        }
    }
}
