using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float rotateSpeed = 50f;
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
    }

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            gm.GetComponent<GameManager>().CollectablePickedUp();
            Destroy(gameObject);
        }
    }
}
