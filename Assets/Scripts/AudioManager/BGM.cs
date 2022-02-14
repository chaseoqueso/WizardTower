using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{

	// Start is called before the first frame update
	void Start()
    {
        AudioManager.instance.Play("TitleSong");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
