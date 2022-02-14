using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLife : MonoBehaviour
{
    public int health = 3;
    public GameOverUI gameOver;
    public GameObject healthCrystalPrefab;

    private List<GameObject> healthCrystals;

    // Start is called before the first frame update
    void Start()
    {
        healthCrystals = new List<GameObject>();
        for(int i = 0; i < health; ++i)
        {
            healthCrystals.Add(Instantiate(healthCrystalPrefab, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < healthCrystals.Count; ++i)
        {
            float angle = Time.time + i * Mathf.PI * 2 / 3;
            healthCrystals[i].transform.position = transform.position + new Vector3(Mathf.Cos(angle) * 4, 10, Mathf.Sin(angle) * 4);
        }
    }
    public void TakeDamage()
    {
        // if tower collide with enemy
        AudioManager.instance.Play("TowerSound");
        health--;

        Debug.Log("Took a Hit! Health = " + health);
        if (health == 0)
        {
        AudioManager.instance.Play("DefeatSound");
        //gameOver
        GameManager.instance.timeSinceLevel = Time.timeSinceLevelLoad;
            gameOver.ToggleGameOverUI(true);
        }

        Destroy(healthCrystals[0]);
        healthCrystals.RemoveAt(0);
    }
}
