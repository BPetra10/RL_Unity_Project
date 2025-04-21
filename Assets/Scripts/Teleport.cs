using UnityEngine;

public class Teleport : MonoBehaviour
{
    private GameObject playerRef;
    private GameObject teleportLeft;
    private GameObject teleportRight;
    private GameObject teleportTop;

    float portalOffset = -2f; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Agent");

        if (playerRef == null)
        {
            Debug.Log("No player ref Found!");
        }

        teleportLeft = GameObject.FindGameObjectWithTag("Portal_left");
        teleportRight = GameObject.FindGameObjectWithTag("Portal_right");
        teleportTop = GameObject.FindGameObjectWithTag("Portal_top");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            switch (gameObject.tag)
            {
                case "Portal_left":
                case "Portal_right":
                    TeleportToTop(other);
                    break;

                case "Portal_top":
                    TeleportToRandom(other);
                    break;

                default:
                    Debug.Log("Unexpected portal encountered!");
                    break;
            }
        }
    }

    // Teleport the agent to the Top portal, but with offset it slightly in front of the portal
    private void TeleportToTop(Collider agent)
    {
        Vector3 spawnPosition = teleportTop.transform.position;
        spawnPosition.z += portalOffset;

        agent.transform.position = spawnPosition;
        playerRef.GetComponent<AgenController>().teleport(spawnPosition);
        Debug.Log("Teleported to Top!");
    }

    // Teleport the agent to either Left or Right portal randomly, with some offset
    private void TeleportToRandom(Collider agent)
    {
        GameObject randomTeleport = Random.Range(0, 2) == 0 ? teleportLeft : teleportRight;
        Vector3 spawnPosition = randomTeleport.transform.position;

        if (randomTeleport == teleportLeft)
        {
            spawnPosition.x -= portalOffset;  
            spawnPosition.z += portalOffset;

        }
        else 
        {
            spawnPosition.x += portalOffset;
            spawnPosition.z += portalOffset;
        }

        agent.transform.position = spawnPosition;
        playerRef.GetComponent<AgenController>().teleport(spawnPosition);
        Debug.Log("Teleported to " + randomTeleport.name);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
