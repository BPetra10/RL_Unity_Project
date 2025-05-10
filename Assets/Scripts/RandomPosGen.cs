using System.Collections.Generic;
using UnityEngine;

public class RandomPosGen
{
    private List<Renderer> planes = new List<Renderer>();
    private float margin;
    private Transform mapParent;

    public RandomPosGen(GameObject map, float edgeMargin = 1.5f)
    {
        mapParent = map.transform;
        margin = edgeMargin;
        CollectPlanes(mapParent);
    }

    private void CollectPlanes(Transform mapParent)
    {
        planes.Clear();
        foreach (Transform child in mapParent)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null)
            {
                planes.Add(rend);
            }
        }

        if (planes.Count == 0)
        {
            Debug.LogWarning("Nincs plane a megadott Map alatt.");
        }
    }

    public Vector3 GetRandomPosition()
    {
        if (planes.Count == 0)
        {
            Debug.LogWarning("Nincs elérhető plane.");
            return Vector3.zero;
        }

        Renderer selected = planes[Random.Range(0, planes.Count)];
        Bounds bounds = selected.bounds;

        float minX = bounds.min.x + margin;
        float maxX = bounds.max.x - margin;
        float minZ = bounds.min.z + margin;
        float maxZ = bounds.max.z - margin;

        if (minX > maxX || minZ > maxZ)
        {
            Debug.LogWarning("Margin túl nagy, nincs hely spawnra!");
            return bounds.center;
        }

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        return new Vector3(randomX, 1.2f, randomZ);
    }
}