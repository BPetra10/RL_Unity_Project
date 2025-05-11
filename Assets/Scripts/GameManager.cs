using System.Linq;
using Unity.MLAgents;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject collectablePrefab;
    public GameObject enemyTurretPrefab;
    [SerializeField] public GameObject mapObj;
    private int initialEnemyCount = 1;
    private int counter = 0;
    private int goalNumber = 10;
    private float minDistance = 5f;
    private float higherReward = 30f;
    private float maxReward = 80f;

    private EnvironmentController env;

    void Start()
    {
        env = GetComponentInParent<EnvironmentController>();
        //SpawnCollectable();


        //TÖBB VILLÁM GYORSABB TANULÁSHOZ (3) Resetben is ki kell kommenteleni
        for (int i = 0; i < 3; i++)
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
        Agent agent = env.GetAgent().GetComponent<Agent>();
        if (agent != null)
        {
            agent.AddReward(higherReward);
        }

        counter++;
        higherReward = Mathf.Min(higherReward + 10f, maxReward);

        Debug.LogError("Reward for collectable pick up: " + higherReward);

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
        RandomPosGen posGen = new RandomPosGen(mapObj);
        Vector3 spawnPos = posGen.GetRandomPosition();
        Instantiate(collectablePrefab, spawnPos, Quaternion.identity, env.transform);
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPos;
     
        RandomPosGen posGen = new RandomPosGen(mapObj);
        spawnPos = posGen.GetRandomPosition();
       

        spawnPos.y -= 0.7f;
        Instantiate(enemyTurretPrefab, spawnPos, Quaternion.identity, env.transform);
    }

   

    public void ResetEnvironment()
    {
        counter = 0;
        foreach (Transform child in env.transform)
        {
            if (child.CompareTag("Thunder")) Destroy(child.gameObject);
        }

       
        foreach (Transform child in env.transform)
        {
            if (child.CompareTag("EnemyTurret")) Destroy(child.gameObject);
        }

       
        foreach (Transform child in env.transform)
        {
            if (child.CompareTag("Projectile")) Destroy(child.gameObject);
        }
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }

        //SpawnCollectable();


        for (int i = 0; i < 3; i++)
        {

            SpawnCollectable();
        }
        higherReward = 30f;
    }
}