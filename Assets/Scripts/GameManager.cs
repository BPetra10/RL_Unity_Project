using Unity.MLAgents;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject collectablePrefab;
    public GameObject enemyTurretPrefab;
    [SerializeField] public GameObject mapObj;
    private int initialEnemyCount = 2;
    private int counter = 0;
    private int goalNumber = 10;
    private float minDistance = 5f;
    private float higherReward = 30f;
    private float maxReward = 80f;

    private EnvironmentController env;

    void Start()
    {
        env = GetComponentInParent<EnvironmentController>();
        SpawnCollectable();
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

        Debug.Log("Reward for collectable pick up: " + higherReward);

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
        do
        {
            RandomPosGen posGen = new RandomPosGen(mapObj);
            spawnPos = posGen.GetRandomPosition();
        } while (IsTooClose(spawnPos));

        spawnPos.y -= 0.7f;
        Instantiate(enemyTurretPrefab, spawnPos, Quaternion.identity, env.transform);
    }

    private bool IsTooClose(Vector3 pos)
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("EnemyTurret"))
        {
            if (enemy.transform.IsChildOf(transform) &&
                Vector3.Distance(pos, enemy.transform.position) < minDistance)
                return true;
        }
        return false;
    }

    public void ResetEnvironment()
    {
        counter = 0;
        foreach (Transform child in env.transform)
        {
            if (child.CompareTag("Thunder")) Destroy(child.gameObject);
        }

        // Töröljük az ellenségeket
        foreach (Transform child in env.transform)
        {
            if (child.CompareTag("EnemyTurret")) Destroy(child.gameObject);
        }

        // Töröljük a lövedékeket
        foreach (Transform child in env.transform)
        {
            if (child.CompareTag("Projectile")) Destroy(child.gameObject);
        }
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }
        SpawnCollectable();
        higherReward = 30f;
    }
}