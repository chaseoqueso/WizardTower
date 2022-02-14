using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKILL : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == gameObject.tag)
        {
            AudioManager.instance.Play("HitSound");
            Destroy(transform.parent.gameObject);
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
