using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.5f;
    public Material blue;
    public Material green;
    public Material red;
    public Material yellow;

    public Transform Tower;
    // Start is called before the first frame update
    void Start()
    {
        Tower = GameObject.Find("WizardTowerPillar").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Tower.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "WizardTowerPillar")
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(int height, int color)
    {
        GameObject childObject = transform.GetChild(height ).gameObject;
        childObject.SetActive(true);
        SetColor(childObject, color);
    }

    public void SetColor(GameObject childObject, int color)
    {
        switch(color)
        {
            case 0:
                //blue
                childObject.GetComponent<MeshRenderer>().material = blue;
                childObject.tag = "Blue";
                break;
            case 1:
                //green
                childObject.GetComponent<MeshRenderer>().material = green;
                childObject.tag = "Green";
                break;
            case 2:
                //red
                childObject.GetComponent<MeshRenderer>().material = red;
                childObject.tag = "Red";
                break;
            case 3:
                //yellow
                childObject.GetComponent<MeshRenderer>().material = yellow;
                childObject.tag = "Yellow";
                break;
        }
    }
}
