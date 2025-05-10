using UnityEngine;

public class Teleport : MonoBehaviour
{
    private GameObject playerRef;
    private GameObject teleportLeft;
    private GameObject teleportRight;
    private GameObject teleportTop;
    private EnvironmentController env;

    float portalOffset = -2f;

    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Agent");

        if (playerRef == null)
        {
            Debug.Log("No player ref Found!");
        }



        env = GetComponentInParent<EnvironmentController>();
        if (env == null)
        {
            Debug.LogError("EnvironmentController not found in parent.");
            return;
        }
        teleportLeft = FindChildWithTag(env.transform, "Portal_left");
        teleportRight = FindChildWithTag(env.transform, "Portal_right");
        teleportTop = FindChildWithTag(env.transform, "Portal_top");
        playerRef = FindChildWithTag(env.transform, "Agent");
    }

    private GameObject FindChildWithTag(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
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

    private void TeleportToTop(Collider agent)
    {
        Vector3 spawnPosition = teleportTop.transform.position;
        spawnPosition.z += portalOffset;

        agent.transform.position = spawnPosition;
        playerRef.GetComponent<AgentController>().teleport(spawnPosition);
        Debug.Log("Teleported to Top!");
    }

    private void TeleportToRandom(Collider agent)
    {
        GameObject randomTeleport = Random.Range(0, 2) == 0 ? teleportLeft : teleportRight;
        Vector3 spawnPosition = randomTeleport.transform.position;

        spawnPosition.x += (randomTeleport == teleportLeft) ? -portalOffset : portalOffset;

        agent.transform.position = spawnPosition;
        playerRef.GetComponent<AgentController>().teleport(spawnPosition);
        Debug.Log("Teleported to " + randomTeleport.name);
    }
}
