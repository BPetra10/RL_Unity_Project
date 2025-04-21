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

    private bool attackStarted = false;


    void Start()
    {
        gm = GameObject.Find("GameMananger");
        if (gm == null)
        {
            Debug.LogError("Game Manager object not found!");
        }

        agentTransform = GameObject.FindGameObjectWithTag("Agent").transform;
        if (agentTransform == null)
        {
            Debug.LogError("Player GameObject not found");
        }


        StartCoroutine(FireProjectile());
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, agentTransform.position) <= detectionRange)
        {

            Vector3 direction = agentTransform.position - transform.position;
            direction.y = 0f;
       

            Quaternion lookRotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);

            
            Vector3 newForward = Vector3.Cross(Vector3.up, direction); 
            transform.rotation = Quaternion.LookRotation(-newForward, Vector3.up);

        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            //gm.GetComponent<GameManager>().GameOver(true);
        }
    }

    IEnumerator FireProjectile()
    {

        while (shotsFired < maxShots)
        {
            if (Vector3.Distance(transform.position, agentTransform.position) <= detectionRange)
            {

                yield return new WaitForSeconds(fireRate);
                attackStarted = true;
                Vector3 firePos = new Vector3(transform.position.x, transform.position.y + 1.75f, transform.position.z);
                Instantiate(projectilePrefab, firePos, transform.rotation);
                yield return new WaitForSeconds(1.5f);
                shotsFired++;
                Destroy(gameObject);
            }
            else
            {
                yield return null;
            }
        }


        if (shotsFired >= maxShots)
        {

            gm.GetComponent<GameManager>().SpawnEnemy();
        }

    }
}
