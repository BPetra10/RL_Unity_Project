using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int counter = 0;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private GameObject enemyTurretPrefab;
    private GameObject agentPrefab;
    private float minDistance = 5f;
    private int enemyCount = 0;
    public int initialEnemyCount = 4;

    private int goalNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalNumber = 10;

         SpawnCollectable();
       

        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }

        agentPrefab = GameObject.FindGameObjectWithTag("Agent");

        if (agentPrefab == null)
        {
            Debug.LogError("agent object not found");
        }
    }
    public void CollectablePickedUp()
    {
        Agent agent = agentPrefab?.GetComponent<Agent>();
        if (agent != null)
        {
            agent.AddReward(30f);
        }

        counter++;

        if (counter >= goalNumber)
        {
            agent?.EndEpisode();
        }
        else
        {
            SpawnCollectable();
        }
    }

    private void SpawnCollectable()
    {
        RandomPosGen posGen = new RandomPosGen();
        Vector3 spawnPosition = posGen.GetRandomPosition();
        Instantiate(collectablePrefab, spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    public void SpawnEnemy()
    {
        enemyCount++;

        Vector3 spawnPos;


        do
        {

            RandomPosGen posGen = new RandomPosGen();
            spawnPos = posGen.GetRandomPosition();
            

        }
        while (IsTooCloseToOtherEnemy(spawnPos));

        spawnPos.y= spawnPos.y - 0.7f;
        Instantiate(enemyTurretPrefab, spawnPos, Quaternion.identity);

    }

    private bool IsTooCloseToOtherEnemy(Vector3 pos)
    {
        //TODO Portálba ne essen , a collectable se
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTurret");
        GameObject player = GameObject.FindGameObjectWithTag("Agent");
        GameObject diamond = GameObject.FindGameObjectWithTag("Thunder");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(pos, enemy.transform.position) < minDistance
                && Vector3.Distance(pos, player.transform.position) < minDistance
                && Vector3.Distance(pos, diamond.transform.position) < minDistance)
            {
                return true;
            }
        }
        return false;
    }

    //void Update()
    //{
    //    collectable = GameObject.FindGameObjectWithTag("Thunder");
    //}

    public void ResetEnvironment()
    {
        counter = 0;

        GameObject[] collectables = GameObject.FindGameObjectsWithTag("Thunder");

        if (collectables.Length == 0)
        {
            Debug.LogWarning("No thunder objects found to destroy.");
        }

        foreach (GameObject collectable in collectables)
        {
            Destroy(collectable);
        }

        SpawnCollectable();
    }


}
