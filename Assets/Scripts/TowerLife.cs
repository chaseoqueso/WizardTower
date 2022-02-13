using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerLife : MonoBehaviour
{
    public int health = 3;
    public GameOverUI gameOver;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        // if tower collide with enemy
        if (collision.gameObject.layer == 6)
        {
            health--;
            Debug.Log("Took a Hit! Health = " + health);
            if (health == 0)
            {
                //gameOver
                GameManager.instance.timeSinceLevel = Time.timeSinceLevelLoad;
                gameOver.ToggleGameOverUI(true);
            }
        }
        else
        {
            Debug.Log("Something Fucked up");
        }
    }
}
