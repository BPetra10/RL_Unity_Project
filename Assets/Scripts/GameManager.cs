using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int counter = 0;
    [SerializeField] private GameObject collectablePrefab;

    private int goalNumber;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalNumber = 10;
        for (int i = 0; i < goalNumber; i++)
        {
            SpawnCollectable();
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
    void Update()
    {
       
    }
}
