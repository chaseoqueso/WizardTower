using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKILL : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == gameObject.tag)
        {
            AudioManager.instance.Play("HitSound");
            Destroy(gameObject);
            switch (other.tag)
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
}
