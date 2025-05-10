using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject agent;
    public GameObject map;
    public GameObject teleportLeft;
    public GameObject teleportRight;
    public GameObject teleportTop;

    public GameObject GetAgent() => agent;
    public GameManager GetGameManager() => gameManager;
    public GameObject GetMap() => map;
}
