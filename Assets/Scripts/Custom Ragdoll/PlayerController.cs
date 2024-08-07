using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public float speed;
    public float strafeSpeed;
    public float jumpForce;

    public Rigidbody hips;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", true);
                hips.AddForce(hips.transform.forward * speed * 1.5f);
            }
            else
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                hips.AddForce(hips.transform.forward * speed);
            }

        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isSideSteppingLeft", true);
            hips.AddForce(- hips.transform.right * strafeSpeed);
        }
        else
        {
            animator.SetBool("isSideSteppingLeft", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalking", true);
            hips.AddForce(- hips.transform.forward * speed);
        }else if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isSideSteppingRight", true);
            hips.AddForce(hips.transform.right * strafeSpeed);
        }
        else
        {
            animator.SetBool("isSideSteppingRight", false);
        }

        if(Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                hips.AddForce(new Vector3(0, jumpForce, 0));
                isGrounded = false;
            }
        }
    }
}
