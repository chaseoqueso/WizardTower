using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.5f;
    
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
        }
        else if (height == 2)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (height == 3)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (height == 4)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(2).gameObject.SetActive(true);
            transform.GetChild(3).gameObject.SetActive(true);
        }
    }
}
