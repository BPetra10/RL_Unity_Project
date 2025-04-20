using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float rotateSpeed = 50f;

    private GameObject gm;

    void Start()
    {
        gm = GameObject.Find("GameMananger");

        if (gm == null)
        {
            Debug.LogError("Game Manager object not found!");
        }
    }

    void Update()
    {
        // A gy�m�nt forgat�sa
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);
    }

    // Ha az �gens hozz��r a gy�m�nthoz
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Agent"))
        {
            gm.GetComponent<GameManager>().CollectablePickedUp();

            Destroy(gameObject);
        }
    }
}
