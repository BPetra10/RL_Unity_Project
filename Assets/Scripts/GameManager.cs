using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int counter = 0;
    [SerializeField] private GameObject collectablePrefab;
    [SerializeField] private GameObject enemyTurretPrefab;
    private float minDistance = 5f;
    private int enemyCount = 0;
    public int initialEnemyCount = 4;

    private int goalNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalNumber = 10;
        for (int i = 0; i < goalNumber; i++)
        {
            SpawnCollectable();
        }

        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }
    }
    public void CollectablePickedUp()
    {
        SpawnCollectable(); 
        if (counter == goalNumber) 
        {
            //TODO
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
}
