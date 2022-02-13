using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int spawnListNum;
    private int randSpawn;
    private int lastRand;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float spawnCooldown = 6f;
    [SerializeField]
    private float spawnDelay = 6f;

    //This is for phase changes. Phase 1 -> Phase 2 -> Phase 3.....
    // Within phase changes you can change the spawn rate and other enemy variables.
    [SerializeField]
    private float p1 = 60f;
    [SerializeField]
    private float p2 = 60f;
    [SerializeField]
    private float p3 = 40f;
    // Start is called before the first frame update
    void Start()
    {
        spawnListNum = transform.childCount;
        StartCoroutine(phase1());

    }

    // Update is called once per frame
    void Update()
    {
        if (spawnDelay > 0)
        {
            spawnDelay -= Time.deltaTime;
        }
        else if (spawnDelay <= 0)
        {
            randSpawn = Random.Range(0, spawnListNum);
            while (randSpawn == lastRand)
            {
                randSpawn = Random.Range(0, spawnListNum);
            }
            GameObject enemy = Instantiate(enemyPrefab, transform.GetChild(randSpawn));
            //enemy modification script
            spawnDelay = spawnCooldown;
            lastRand = randSpawn;
        }
    }
    IEnumerator phase1()
    {
        yield return new WaitForSeconds(p1);
        //number changes
        spawnCooldown = 5f;
        Debug.Log("p1 done");
        StartCoroutine(phase2());
    }
    IEnumerator phase2()
    {
        yield return new WaitForSeconds(p2);
        //number changes
        spawnCooldown = 4f;
        Debug.Log("p2 done");
        StartCoroutine(phase3());
        
    }
    IEnumerator phase3()
    {
        yield return new WaitForSeconds(p3);
        Debug.Log("p3 done");
        spawnCooldown = 3f;
        //number changes

    }
}

