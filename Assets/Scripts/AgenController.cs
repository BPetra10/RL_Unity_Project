using UnityEngine;

public class AgenController : MonoBehaviour
{
    //Csak az animáció miatt hoztam létre mert elvileg Itt kell bűvészkedni.
    private Animator animator;
    private GameObject gm;
    bool isWalking ;

    void Start()
    {
        gm = GameObject.Find("GameMananger");
        if (gm == null)
        {
            Debug.LogError("Game Manager object not found");
        }

        animator = GameObject.FindAnyObjectByType<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (animator != null)
        {
            animator.SetBool("Run_b", isWalking);
        }
    }
}
