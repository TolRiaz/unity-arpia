using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int[] maxCount;
    public int[] enemyCount;
    public float[] spawnTime;
    public float[] curTime;
    public Vector2 pos;
    public bool[] isSpawn;
    public GameObject[] enemy;


    public GameObject[] breakableEntity;
    public int[] spawnRandomRange = new int[4];

    private int enemyNumber;

    public int totalMobCount;

    public static SpawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        totalMobCount = enemy.Length;

        maxCount = new int[totalMobCount];
        enemyCount = new int[totalMobCount];
        spawnTime = new float[totalMobCount];
        curTime = new float[totalMobCount];
        isSpawn = new bool[totalMobCount];


        spawnTime[0] = 3f;
        spawnTime[1] = 5f;
        spawnTime[2] = 10f;

        for (int i = 0; i < maxCount.Length; i++)
        {
            maxCount[i] = 5;
        }
    }

    private void Update()
    {
        if (PlayerManager.instance.location == Map.FIELD)
        {
            forestSpawnEnvironment();
        }
    }

    private void forestSpawnEnvironment()
    {
        enemyNumber = 0;
        if (curTime[enemyNumber] >= spawnTime[enemyNumber] && enemyCount[enemyNumber] < maxCount[enemyNumber])
        {
            int x = Random.Range(spawnRandomRange[0], spawnRandomRange[1]);
            int y = Random.Range(spawnRandomRange[2], spawnRandomRange[3]);
            spawnEnemy(enemyNumber, x - 1000, y + 495);
        }

        enemyNumber = 1;
        if (curTime[enemyNumber] >= spawnTime[enemyNumber] && enemyCount[enemyNumber] < maxCount[enemyNumber])
        {
            int x = Random.Range(spawnRandomRange[0], spawnRandomRange[1]);
            int y = Random.Range(spawnRandomRange[2], spawnRandomRange[3]);
            spawnEnemy(enemyNumber, x - 1050, y + 490);
        }

        enemyNumber = 2;
        if (curTime[enemyNumber] >= spawnTime[enemyNumber] && enemyCount[enemyNumber] < maxCount[enemyNumber])
        {
            int x = Random.Range(spawnRandomRange[0], spawnRandomRange[1]);
            int y = Random.Range(spawnRandomRange[2], spawnRandomRange[3]);
            spawnEnemy(enemyNumber, x - 950, y + 490);
        }

        for (int i = 0; i < curTime.Length; i++)
        {
            curTime[i] += Time.deltaTime;
        }
    }

    public void spawnEnemy(int enemyNumber, int posX, int posY)
    {
        curTime[enemyNumber] = 0;
        enemyCount[enemyNumber]++;
        pos.x = posX;
        pos.y = posY;
        Instantiate(enemy[enemyNumber], pos, Quaternion.identity);
    }

    public void spawnBreakableEntity(int entityNumber, int posX, int posY)
    {
        pos.x = posX;
        pos.y = posY;
        Instantiate(breakableEntity[entityNumber], pos, Quaternion.identity);
    }
}
