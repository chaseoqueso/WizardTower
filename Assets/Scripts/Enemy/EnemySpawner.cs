using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int spawnListNum;
    [SerializeField]
    private GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        spawnListNum = transform.childCount;
        Debug.Log(spawnListNum);
        test();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void test()
    {
        foreach (Transform child in transform)
            Debug.Log(child.position);
    }
}
