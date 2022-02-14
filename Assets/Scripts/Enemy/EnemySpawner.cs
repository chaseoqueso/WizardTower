using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private int spawnListNum;
    private int randSpawn;
    private int lastRand;
    private int randHeight;
    [SerializeField]
    private GameObject enemyPrefab;
    //Spawn rate
    [SerializeField]
    private float spawnCooldown = 6f;
    [SerializeField]
    private float spawnDelay = 6f;
    [SerializeField]
    private float spawnRateIncreasePerPhase = 1.25f;

    //This is for phase changes. Phase 1 duraton -> Phase 2 -> Phase 3.....
    // Within phase changes you can change the spawn rate and other variables.
    [SerializeField]
    private float phase1Duration = 30f;
    [SerializeField]
    private float phase2Duration = 60f;
    [SerializeField]
    private float phase3Duration = 60f;

    // Height Probability
    [SerializeField]
    private int height1Probability = 25;
    [SerializeField]
    private int height2Probability = 30;
    [SerializeField]
    private int height3Probability = 30;
    [SerializeField]
    private int height4Probability = 15;

    private int totalProbability;

    private Queue<WizardType> enemyTypeQueue;

    void Start()
    {
        enemyTypeQueue = new Queue<WizardType>();
        enemyTypeQueue.Enqueue(WizardType.redCone);
        enemyTypeQueue.Enqueue(WizardType.blueSquare);
        enemyTypeQueue.Enqueue(WizardType.yellowRound);
        enemyTypeQueue.Enqueue(WizardType.greenDiamond);

        for(int _ = 0; _ < Random.Range(0, (int)WizardType.enumSize); ++_)
        {
            enemyTypeQueue.Enqueue(enemyTypeQueue.Dequeue());
        }

        spawnListNum = transform.childCount;
        totalProbability = height1Probability;
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
            //random spawn loc
            randSpawn = Random.Range(0, spawnListNum);
            while (randSpawn == lastRand)
            {
                randSpawn = Random.Range(0, spawnListNum);
            }

            WizardType enemyType = enemyTypeQueue.Dequeue();

            //random spawn height
            GameObject enemy = Instantiate(enemyPrefab, transform.GetChild(randSpawn));
            randHeight = Random.Range(0, totalProbability);
            if(randHeight < height1Probability)
            {
                enemy.GetComponent<EnemyAI>().Initialize(0, enemyType);
            }
            else if (randHeight < height1Probability + height2Probability)
            {
                enemy.GetComponent<EnemyAI>().Initialize(1, enemyType);
            }
            else if (randHeight < height1Probability + height2Probability + height3Probability)
            {
                enemy.GetComponent<EnemyAI>().Initialize(2, enemyType);
            }
            else
            {
                enemy.GetComponent<EnemyAI>().Initialize(3, enemyType);
            }

            enemyTypeQueue.Enqueue(enemyType);

            //enemy modification script
            spawnDelay = spawnCooldown;
            lastRand = randSpawn;
        }
    }
    IEnumerator phase1()
    {
        yield return new WaitForSeconds(phase1Duration);
        totalProbability = height1Probability + height2Probability;
        spawnCooldown /= spawnRateIncreasePerPhase;
        StartCoroutine(phase2());
    }
    IEnumerator phase2()
    {
        yield return new WaitForSeconds(phase2Duration);
        totalProbability = height1Probability + height2Probability + height3Probability;
        spawnCooldown /= spawnRateIncreasePerPhase;
        StartCoroutine(phase3());
        
    }
    IEnumerator phase3()
    {
        yield return new WaitForSeconds(phase3Duration);
        totalProbability = height1Probability + height2Probability + height3Probability + height4Probability;
        spawnCooldown /= spawnRateIncreasePerPhase;
    }
}

