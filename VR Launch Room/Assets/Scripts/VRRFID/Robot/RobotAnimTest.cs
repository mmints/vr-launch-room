using UnityEngine;

public class RobotAnimTest : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            animator.SetBool("Walk_Anim", true);
        }
        if (Input.GetKeyDown("s"))
        {
            animator.SetBool("Walk_Anim", false);
        }
        
        if (Input.GetKeyDown("d"))
        {
            animator.SetBool("Open_Anim", true);
        }
        if (Input.GetKeyDown("f"))
        {
            animator.SetBool("Open_Anim", true);
        }
        if (Input.GetKeyDown("j"))
        {
            animator.SetBool("Tap_Anim", true);
        }
        if (Input.GetKeyDown("k"))
        {
            animator.SetBool("Tap_Anim", false);
        }
        
    }
}
