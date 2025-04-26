using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentController : Agent
{

    public float speed = 5f;


    public Transform TargetTransform;
    private CharacterController characterController;

    private enum ACTIONS
    {
        LEFT = 0,
        FORWARD = 1,
        RIGHT = 2,
        BACKWARD = 3
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0, 0.5f, -10);

        TargetTransform = GameObject.FindGameObjectWithTag("Thunder").transform;
        
        if (TargetTransform == null)
        {
            Debug.LogError("Target GameObject not found");
        }
    }

  
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);

        sensor.AddObservation(TargetTransform.localPosition.x);
        sensor.AddObservation(TargetTransform.localPosition.z);

    }

    public void teleport(Vector3 newPos)
    {
        characterController.enabled = false;
        transform.position = newPos;
        characterController.enabled = true;
    }
}
