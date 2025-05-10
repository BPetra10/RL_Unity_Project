using System.Collections;
using UnityEngine;

public class EnemyTurretController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float detectionRange = 50f;
    private GameObject gm;
    private int shotsFired = 0;
    private int maxShots = 1;
    private Transform agentTransform;
    private Transform environmentTransform;

    void Start()
    {
        var envController = GetComponentInParent<EnvironmentController>();
        if (envController == null)
        {
            Debug.LogError("EnvironmentController not found in parent hierarchy.");
            return;
        }

        agentTransform = envController.GetAgent().transform;
        gm = envController.GetGameManager().gameObject;

        agentTransform = envController.GetAgent().transform;
        environmentTransform = envController.transform;
        StartCoroutine(FireProjectile());
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, agentTransform.position) <= detectionRange)
        {
            Vector3 direction = agentTransform.position - transform.position;
            direction.y = 0f;

            Vector3 newForward = Vector3.Cross(Vector3.up, direction);
            transform.rotation = Quaternion.LookRotation(-newForward, Vector3.up);
        }
    }

    IEnumerator FireProjectile()
    {
        while (shotsFired < maxShots)
        {
            if (Vector3.Distance(transform.position, agentTransform.position) <= detectionRange)
            {
                yield return new WaitForSeconds(fireRate);
                Vector3 firePos = new Vector3(transform.position.x, transform.position.y + 1.75f, transform.position.z);
                Instantiate(projectilePrefab, firePos, transform.rotation, environmentTransform);
                yield return new WaitForSeconds(1.5f);
                shotsFired++;
                Destroy(gameObject);
            }
            else yield return null;
        }

        if (shotsFired >= maxShots)
        {
            gm.GetComponent<GameManager>().SpawnEnemy();
        }
    }
}
