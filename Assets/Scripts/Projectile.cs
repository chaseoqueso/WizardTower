using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private const float speed = 25f;

    public GameObject redGem;
    public GameObject blueGem;
    public GameObject yellowGem;
    public GameObject greenGem;
    public Vector3 direction;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularVelocity = Random.onUnitSphere * 5;
    }

    public void setGemFromTag()
    {
        switch(tag)
        {
            case "Red":
                Instantiate(redGem, transform).transform.localScale /= 5;
                break;
            case "Blue":
                Instantiate(blueGem, transform).transform.localScale /= 5;
                break;
            case "Yellow":
                Instantiate(yellowGem, transform).transform.localScale /= 5;
                break;
            case "Green":
                Instantiate(greenGem, transform).transform.localScale /= 5;
                break;
        }
    }

    void FixedUpdate()
    {
        rb.velocity = direction * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
            Destroy(gameObject);
        
    }
}
