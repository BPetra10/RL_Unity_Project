using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float flyspeed;
    private Transform agentTransform;
    private GameObject gm;


    void Start()
    {
        agentTransform = GameObject.FindGameObjectWithTag("Agent").transform;
        gm = GameObject.Find("GameManager");
        if (gm == null)
        {
            Debug.LogError("Game Manager object not found");
        }



        if (agentTransform == null)
        {
            Debug.LogError("Player GameObject not found");
        }

        RotateTowardsPlayer();
    }

    void Update()
    {
        //TODO-Lábához lövi a lövedéket-JAVÍT
        transform.Translate(Vector3.forward * flyspeed * Time.deltaTime);
    }

    private void RotateTowardsPlayer()
    {
        if (agentTransform != null)
        {
            transform.LookAt(agentTransform);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
           //TODO - büntetés
            Destroy(gameObject);
        }
    }
}
