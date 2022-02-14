using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float speed = 15f;

    public Vector3 direction;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        switch (gameObject.tag)
        {
            case "Red":
                AudioManager.instance.Play("RedHit");
                break;
            case "Blue":
                AudioManager.instance.Play("BlueHit");
                break;
            case "Yellow":
                AudioManager.instance.Play("YellowHit");
                break;
            case "Green":
                AudioManager.instance.Play("GreenHit");
                break;
        }
    }
}
