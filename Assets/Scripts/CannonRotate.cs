using UnityEngine;

public class CannonRotate : MonoBehaviour
{
    [SerializeField] private float detectionRange = 50f;
    private GameObject gm;
    private Transform agentTransform;



    void Start()
    {
        gm = GameObject.Find("GameMananger");
        if (gm == null)
        {
            Debug.LogError("Game Manager object not found!");
        }

        agentTransform = GameObject.FindGameObjectWithTag("Agent").transform;
        if (agentTransform == null)
        {
            Debug.LogError("Player GameObject not found");
        }

    }

    void Update()
    {
        if (Vector3.Distance(transform.position, agentTransform.position) <= detectionRange)
        {

            //Vector3 direction = agentTransform.position - transform.position;
            //direction.y = 0f;



            //Vector3 newForward = Vector3.Cross(Vector3.up, direction);
            //Quaternion baseRotation = Quaternion.LookRotation(-newForward, Vector3.up);


            //Quaternion offset = Quaternion.AngleAxis(-37f, Vector3.up);
            //transform.rotation = baseRotation * offset;

            Vector3 direction = agentTransform.position - transform.position;
            float distance = direction.magnitude;

            if (distance <= detectionRange)
            {
                // Vízszintes irány (XZ sík)
                Vector3 flatDirection = direction;
                flatDirection.y = 0f;
                flatDirection.Normalize();

                // Alap rotáció, hogy az X tengely nézzen az ágens felé
                Vector3 newForward = Vector3.Cross(Vector3.up, flatDirection);
                Quaternion baseRotation = Quaternion.LookRotation(-newForward, Vector3.up);

                // Y tengely körüli elfordítás (mint eddig)


                // Kiszámoljuk az előre dőlés szögét: függ a magasságkülönbségtől és a távolságtól
                float verticalAngle = Mathf.Atan2(direction.y, new Vector2(direction.x, direction.z).magnitude) * Mathf.Rad2Deg;

                Quaternion yOffset = Quaternion.AngleAxis(-37f, Vector3.up);


                Quaternion xTilt = Quaternion.AngleAxis(verticalAngle, Vector3.forward);
                // X tengely körüli dőlés (fej lehajtása)
                if (verticalAngle < -9f)
                {
                     xTilt = Quaternion.AngleAxis(-9f, Vector3.forward);
                }

                // Végső rotáció összeállítása
                transform.rotation = baseRotation * xTilt *yOffset ;

            }

        }


    }

}
