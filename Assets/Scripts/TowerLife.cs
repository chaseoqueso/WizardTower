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
        

    }
}
