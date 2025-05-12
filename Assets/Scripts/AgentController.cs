using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{

    public float speed = 5f;
    private Animator animator;

    public Transform TargetTransform;
    private EnvironmentController env;
    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        BACKWARD = 3
    }

    public override void OnEpisodeBegin()
    {

        if (env == null)
        {
            env = GetComponentInParent<EnvironmentController>();
            if (env == null)
            {
                Debug.LogError("EnvironmentController not found in parent.");
                return;
            }
        }
        env.GetGameManager().ResetEnvironment();

        transform.localPosition = new Vector3(0, -4f, 0);

        GameObject closestThunder = FindClosestWithTag("Thunder");
        TargetTransform = closestThunder != null ? closestThunder.transform : null;
        if (TargetTransform == null)
        {
            Debug.LogError("[ENV] Thunder GameObject not found in this environment.");
        }
        else
        {
            Debug.LogWarning("Target Transform Found:" + TargetTransform);
        }
        animator = GetComponentInParent<Animator>();
    }


    public override void CollectObservations(VectorSensor sensor)
    {

        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);


        sensor.AddObservation(transform.forward.x);
        sensor.AddObservation(transform.forward.z);
        

        GameObject closestThunder = FindClosestWithTag("Thunder");
        TargetTransform = closestThunder != null ? closestThunder.transform : null;
        if (TargetTransform != null)
        {
            Vector3 relativeTargetPos = TargetTransform.localPosition - transform.localPosition;
            sensor.AddObservation(relativeTargetPos.x);
            sensor.AddObservation(relativeTargetPos.z);
            //Debug.Log($"[OBSERVE] Thunder relative pos: {relativeTargetPos}");
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            Debug.LogWarning($"[OBSERVE] Thunder is null.");
        }

        // Lövedék (Projectile) legközelebbi pozíció
        GameObject closestProjectile = FindClosestWithTag("Projectile");
        if (closestProjectile != null)
        {
            Vector3 relativeProjectilePos = closestProjectile.transform.localPosition - transform.localPosition;
            sensor.AddObservation(relativeProjectilePos.x);
            sensor.AddObservation(relativeProjectilePos.z);
            //Debug.Log($"[OBSERVE] Projectal is NOOOOOT null.");
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
          //  Debug.LogWarning($"[OBSERVE] Projectal is null.");
        }

        // Ellenség (Enemy) legközelebbi pozíció
        GameObject closestEnemy = FindClosestWithTag("EnemyTurret");
        if (closestEnemy != null)
        {
            Vector3 relativeEnemyPos = closestEnemy.transform.localPosition - transform.localPosition;
            sensor.AddObservation(relativeEnemyPos.x);
            sensor.AddObservation(relativeEnemyPos.z);
           // Debug.Log($"[OBSERVE] Turret is NOOOOOT null.");
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
            Debug.LogWarning($"[OBSERVE] Turret is null.");
        }
    }

    private GameObject FindClosestWithTag(string tag)
    {
        GameObject[] objs = env.GetComponentsInChildren<Transform>(true)
                                .Where(t => t.CompareTag(tag))
                                .Select(t => t.gameObject)
                                .ToArray();

        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject obj in objs)
        {
            float dist = Vector3.Distance(currentPos, obj.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = obj;
            }
        }

        return closest;
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

        var horizontal = Input.GetAxisRaw("Horizontal"); 
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == -1)
        {
            actions[0] = (int)ACTIONS.LEFT;
        }
        else if (horizontal == +1)
        {
            actions[0] = (int)ACTIONS.RIGHT;
        }
        else if (vertical == -1)
        {
            actions[0] = (int)ACTIONS.BACKWARD;
        }
        else if (vertical == +1)
        {
            actions[0] = (int)ACTIONS.FORWARD;
        }
        else
        {
            actions[0] = (int)ACTIONS.FORWARD;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        var actionTaken = actions.DiscreteActions[0];
        bool isMoving = true;

        switch (actionTaken)
        {

            case (int)ACTIONS.FORWARD:
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case (int)ACTIONS.LEFT:
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case (int)ACTIONS.RIGHT:
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case (int)ACTIONS.BACKWARD:
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
        Debug.Log("Action received: " + actionTaken);

        if (isMoving)
        {
            transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        }

        AddReward(-0.01f);

        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            AddReward(-10f);
            //Debug.LogWarning($"Wall punishment applied.");
            EndEpisode();
        }
    }

    public void teleport(Vector3 newPos)
    {
        transform.position = newPos;
    }
}
