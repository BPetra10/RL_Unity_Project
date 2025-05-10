using System.Collections;
using Unity.MLAgents;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float flyspeed;
    private Transform agentTransform;
    private GameObject gm;

    void Start()
    {
        var envController = GetComponentInParent<EnvironmentController>();
        if (envController == null)
        {
            Debug.LogError("EnvironmentController not found in parent hierarchy.");
            return;
        }

        gm = envController.GetGameManager().gameObject;
        agentTransform = envController.GetAgent().transform;
        if (gm == null) Debug.LogError("Game Manager object not found");
        if (agentTransform == null) Debug.LogError("Player GameObject not found");

        RotateTowardsPlayer();
        Destroy(gameObject, 4f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * flyspeed * Time.deltaTime);
    }

    private void RotateTowardsPlayer()
    {
        if (agentTransform != null)
        {
            Vector3 targetPosition = agentTransform.position + new Vector3(0, 0.5f, 0);
            transform.LookAt(targetPosition);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            var agent = other.GetComponent<Agent>();
            if (agent != null)
            {
                agent.AddReward(-10f);
                Debug.LogError("projectal hit punishment ");
                agent.EndEpisode();
            }
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
