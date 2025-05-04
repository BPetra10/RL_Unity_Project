using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{

    public float speed = 5f;


    public Transform TargetTransform;

    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        BACKWARD = 3
    }

    public override void OnEpisodeBegin()
    {
        FindAnyObjectByType<GameManager>().ResetEnvironment();

        transform.localPosition = new Vector3(0, 0.5f, -10);

        TargetTransform = GameObject.FindGameObjectWithTag("Thunder").transform;
        
        if (TargetTransform == null)
        {
            Debug.LogError("Target GameObject not found");
        }
    }


    public override void CollectObservations(VectorSensor sensor)
    {
        // Thunder pozíció
        if (TargetTransform != null)
        {
            Vector3 relativeTargetPos = TargetTransform.localPosition - transform.localPosition;
            sensor.AddObservation(relativeTargetPos.x);
            sensor.AddObservation(relativeTargetPos.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }

        // Lövedék (Projectile) legközelebbi pozíció
        GameObject closestProjectile = FindClosestWithTag("Projectile");
        if (closestProjectile != null)
        {
            Vector3 relativeProjectilePos = closestProjectile.transform.localPosition - transform.localPosition;
            sensor.AddObservation(relativeProjectilePos.x);
            sensor.AddObservation(relativeProjectilePos.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }

        // Ellenség (Enemy) legközelebbi pozíció
        GameObject closestEnemy = FindClosestWithTag("EnemyTuret");
        if (closestEnemy != null)
        {
            Vector3 relativeEnemyPos = closestEnemy.transform.localPosition - transform.localPosition;
            sensor.AddObservation(relativeEnemyPos.x);
            sensor.AddObservation(relativeEnemyPos.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    private GameObject FindClosestWithTag(string tag)
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
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

        // The GetAxisRaw() function returns values from the [-1,1]
        // interval
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

        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);
        AddReward(-0.01f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    public void teleport(Vector3 newPos)
    {
        transform.position = newPos;
    }
}
