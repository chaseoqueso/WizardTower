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
    public void SetHeight(int height)
    {
        if (height == 1)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            SetColor(transform.GetChild(0).gameObject);
        }
        else if (height == 2)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            SetColor(transform.GetChild(1).gameObject);
        }
        else if (height == 3)
        {
            transform.GetChild(2).gameObject.SetActive(true);
            SetColor(transform.GetChild(2).gameObject);
        }
        else if (height == 4)
        {
            transform.GetChild(3).gameObject.SetActive(true);
            SetColor(transform.GetChild(3).gameObject);
        }
    }
    public void SetColor(GameObject test)
    {
        int color = Random.Range(1, 5);
        switch(color)
        {
            case 1:
                //blue
                test.GetComponent<MeshRenderer>().material = blue;
                test.tag = "Blue";
                break;
            case 2:
                //green
                test.GetComponent<MeshRenderer>().material = green;
                test.tag = "Green";
                break;
            case 3:
                //red
                test.GetComponent<MeshRenderer>().material = red;
                test.tag = "Red";
                break;
            case 4:
                //yellow
                test.GetComponent<MeshRenderer>().material = yellow;
                test.tag = "Yellow";
                break;
        }
    }
}
