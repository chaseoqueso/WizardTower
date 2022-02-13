using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public int playerNumber = 0;

    private const float moveSpeed = 5;
    private const float pickupDistance = 2.5f;
    private const float shootCooldown = 0.5f;

    public bool enableMovement;
    public bool enableLook;
    public bool beingPickedUp;
    public bool isOnCooldown;
    public float xLookSensitivity = 5;
    public float yLookSensitivity = 5;
    public GameObject projectilePrefab;

    private Coroutine liftRoutine;
    private Coroutine cooldownRoutine;
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

        if (input.wizardInteract && !beingPickedUp && liftRoutine == null)
        {
            Debug.Log("StartRoutine");
            RaycastHit hit;
            Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * pickupDistance, Color.yellow, 1);
            if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, pickupDistance, LayerMask.GetMask("Player")))
            {
                Debug.Log("HitPlayer");
                liftRoutine = StartCoroutine(PickupRoutine(hit.transform.GetComponent<Player>()));
            }
        }

        if(input.shoot && !isOnCooldown)
        {
            GameObject projectile = Instantiate(projectilePrefab, transform.position + transform.rotation * new Vector3(0, 1.5f, 1f), Quaternion.identity);
            projectile.GetComponent<Projectile>().direction = transform.forward;
            cooldownRoutine = StartCoroutine(StartCooldown());
        }
    }

    void FixedUpdate()
    {
        if(enableMovement)
        {
            controller.Move(Quaternion.LookRotation(transform.forward) * moveInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private IEnumerator PickupRoutine(Player otherPlayer)
    {
        CharacterController otherController = otherPlayer.GetComponent<CharacterController>();

        otherPlayer.beingPickedUp = true;
        otherPlayer.enableMovement = false;
        enableMovement = false;

        float pickupProgress = 0;
        while(pickupProgress < 1)
        {
            float angle = Mathf.Lerp(0, Mathf.PI / 2, pickupProgress);
            Vector3 destination = transform.position + transform.rotation * new Vector3(0, Mathf.Sin(angle), Mathf.Cos(angle)) * pickupDistance;

            otherController.Move(destination - otherController.transform.position);
            pickupProgress += 2f * Time.deltaTime;

            yield return null;
        }
        enableMovement = true;

        while (!input.wizardInteract)
        {
            otherPlayer.transform.position = transform.position + Vector3.up * pickupDistance;
            yield return null;
        }

        enableMovement = false;
        while (pickupProgress > 0)
        {
            float angle = Mathf.Lerp(0, Mathf.PI / 2, pickupProgress);
            Vector3 destination = transform.position + transform.rotation * new Vector3(0, Mathf.Sin(angle), Mathf.Cos(angle)) * pickupDistance;

            otherController.Move(destination - otherController.transform.position);
            pickupProgress -= 2f * Time.deltaTime;

            yield return null;
        }

        enableMovement = true;
        otherPlayer.enableMovement = true;
        otherPlayer.beingPickedUp = false;
        liftRoutine = null;
    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(shootCooldown);
        isOnCooldown = false;
    }
}
