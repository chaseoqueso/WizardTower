using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public int playerNumber = 0;

    private const float moveSpeed = 5;
    private const float pickupDistance = 2.5f;
    private const float shootCooldown = 0.5f;
    private const float accel = 0.01f;

    public bool enableMovement;
    public bool enableLook;
    public bool beingPickedUp;
    public bool isOnCooldown;
    public float xLookSensitivity = 5;
    public float yLookSensitivity = 5;
    public GameObject projectilePrefab;
    public WizardType wizardType = WizardType.enumSize;

    private Coroutine liftRoutine;
    private Coroutine cooldownRoutine;
    private InputManager input;
    private CharacterController controller;
    private new Camera camera;
    private Vector3 moveInput;
    private Vector2 lookInput;
    private Vector3 currentVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = GetComponentInChildren<Camera>();
        input = GetComponent<InputManager>();
    }

    public void Initialize(int id)
    {
        //Initialize the camera to be in the correct location and have the current culling mask
        Camera camera = GetComponentInChildren<Camera>();
        camera.rect = new Rect(((id-1) % 2) * 0.5f, ((id-1) / 2) * 0.5f, 0.5f, 0.5f);

        List<string> ignoreLayers = new List<string>(new string[]{"Player1Ignore", "Player2Ignore", "Player3Ignore", "Player4Ignore"});
        ignoreLayers.Remove("Player" + id + "Ignore");

        camera.cullingMask = LayerMask.GetMask("Default", "Player", "UI", "Enemy", "Player" + id + "Only", ignoreLayers[0], ignoreLayers[1], ignoreLayers[2]);

        //Initialize the models to display on the proper layers
        Model model = GetComponentInChildren<Model>();

        foreach(GameObject m in model.thisPlayerOnly)
        {
            m.layer = LayerMask.NameToLayer("Player" + id + "Only");
        }

        foreach(GameObject m in model.thisPlayerIgnore)
        {
            m.layer = LayerMask.NameToLayer("Player" + id + "Ignore");
        }
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

        if(input.shoot && !isOnCooldown && liftRoutine == null)
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
            Vector3 targetVelocity = Quaternion.LookRotation(transform.forward) * moveInput * moveSpeed * Time.fixedDeltaTime;

            if (currentVelocity.x < targetVelocity.x)
            {
                if(currentVelocity.x + accel > targetVelocity.x)
                    currentVelocity.x = targetVelocity.x;
                else
                    currentVelocity.x += accel;
            }

            if (currentVelocity.x > targetVelocity.x)
            {
                if (currentVelocity.x - accel < targetVelocity.x)
                    currentVelocity.x = targetVelocity.x;
                else
                    currentVelocity.x -= accel;
            }

            if(Mathf.Abs(currentVelocity.x) < accel/2)
                currentVelocity.x = 0;

            if (currentVelocity.z < targetVelocity.z)
            {
                if (currentVelocity.z + accel > targetVelocity.z)
                    currentVelocity.z = targetVelocity.z;
                else
                    currentVelocity.z += accel;
            }

            if (currentVelocity.z > targetVelocity.z)
            {
                if (currentVelocity.z - accel < targetVelocity.z)
                    currentVelocity.z = targetVelocity.z;
                else
                    currentVelocity.z -= accel;
            }

            if (Mathf.Abs(currentVelocity.z) < accel / 2)
                currentVelocity.z = 0;

            controller.Move(currentVelocity);

            Debug.Log("Target: " + targetVelocity);
            Debug.Log("Current: " + currentVelocity);
        }
        else
        {
            currentVelocity = Vector3.zero;
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
