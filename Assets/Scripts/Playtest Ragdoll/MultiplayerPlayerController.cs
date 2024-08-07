using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveRagdoll{
public class MultiplayerPlayerController : MonoBehaviour
{

    public enum MovementType
    {
        keyboard,
        controller
    }

    public MovementType movementType;
    public Rigidbody mybody;
    public Animator myAnimator;
    public float jumpSpeed = 400f;
    public bool canJump = true;
    public bool isMoving = false;

    public Camera cam;


    public float currentSpeed = 1000f;
    public float turnspeed = 10f;
    public float walkSpeed = 2500f;
    public float sprintSpeed = 4000f;

    public ConfigurableJoint capsuleJoint;
    public GameObject capsuleObject;
    public bool isLimp = false;

    public bool cameraFollow = true;

    public MultiplayerPhysicsJointController capsuleController;
    public float sprintAnimModifier = 1.4f;


    // Start is called before the first frame update
    void Start()
    {
        mybody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(movementType == MovementType.keyboard)
            {
                if (Input.GetButtonDown("Jump") && canJump)
                {
                    mybody.AddForce(transform.up * jumpSpeed);
                }

                if (Input.GetKeyDown("z"))
                {
                    if (cameraFollow)
                    {
                        cameraFollow = false;
                    }
                    else
                    {
                        cameraFollow = true;
                    }
                }

                if (Input.GetKey("left shift"))
                {
                    currentSpeed = sprintSpeed;

                }
                else
                {
                    currentSpeed = walkSpeed;

                }


                if (isMoving)
                {

                }
                if (transform.position.y <= -10f)
                {

                }

            }//Need to check if controller
            else if (movementType == MovementType.controller)
            {
                if (Input.GetButtonDown("JumpGamepad") && canJump)
                {
                    mybody.AddForce(transform.up * jumpSpeed);
                }

                if (Input.GetButtonDown("CameraGamepad"))
                {
                    if (cameraFollow)
                    {
                        cameraFollow = false;
                    }
                    else
                    {
                        cameraFollow = true;
                    }
                }

                if (Input.GetButton("RunGamepad"))
                {
                    currentSpeed = sprintSpeed;

                }
                else
                {
                    currentSpeed = walkSpeed;

                }


                if (isMoving)
                {

                }
                if (transform.position.y <= -10f)
                {

                }
            }
    }

    void FixedUpdate(){
       
       if(movementType == MovementType.keyboard)
            {
                if (isMoving)
                {

                }

                if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && cameraFollow)
                {
                    transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }


                if (Input.GetKey("w") && !isLimp)
                {
                    mybody.AddForce(transform.forward * currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                if (Input.GetKey("s") && !isLimp)
                {
                    mybody.AddForce(transform.forward * -currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                if (Input.GetKey("d") && !isLimp)
                {
                    mybody.AddForce(transform.right * currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                if (Input.GetKey("a") && !isLimp)
                {
                    mybody.AddForce(transform.right * -currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }


                if (mybody.velocity.magnitude >= 1f)
                {
                    isMoving = true;
                    myAnimator.SetBool("moving", true);

                }
                else
                {
                    isMoving = false;
                    myAnimator.SetBool("moving", false);
                }

                if (Input.GetKey("e"))
                {
                    myAnimator.SetBool("raising", true);
                }
                else
                {
                    myAnimator.SetBool("raising", false);
                }
            }//Controller
            else if(movementType == MovementType.controller)
            {
                if (isMoving)
                {

                }
                //0 is left, 1 is right, 2 is middle
                //Replace with L1/R1
                if ((Input.GetButton("LeftGamepad") || Input.GetButton("RightGamepad")) && cameraFollow)
                {
                    transform.rotation = Quaternion.Euler(cam.transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                }

                //Going forward
                if ((Input.GetAxis("VerticalGamepad") > 0) && !isLimp)
                {
                    mybody.AddForce(transform.forward * currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetButton("LeftGamepad") && !Input.GetButton("RightGamepad") && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                //Going backward
                if ((Input.GetAxis("VerticalGamepad") < 0) && !isLimp)
                {
                    mybody.AddForce(transform.forward * -currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetButton("LeftGamepad") && !Input.GetButton("RightGamepad") && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                //Going right
                if ((Input.GetAxis("HorizontalGamepad") > 0) && !isLimp)
                {
                    mybody.AddForce(transform.right * currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetButton("LeftGamepad") && !Input.GetButton("RightGamepad") && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }
                //Going Left
                if ((Input.GetAxis("HorizontalGamepad") < 0) && !isLimp)
                {
                    mybody.AddForce(transform.right * -currentSpeed * Time.fixedDeltaTime);
                    if (!Input.GetButton("LeftGamepad") && !Input.GetButton("RightGamepad") && cameraFollow)
                    {
                        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
                    }
                }


                if (mybody.velocity.magnitude >= 1f)
                {
                    isMoving = true;
                    myAnimator.SetBool("moving", true);

                }
                else
                {
                    isMoving = false;
                    myAnimator.SetBool("moving", false);
                }

                if (Input.GetButton("RaiseGamepad"))
                {
                    myAnimator.SetBool("raising", true);
                }
                else
                {
                    myAnimator.SetBool("raising", false);
                }
            }


    }

    void OnTriggerEnter(Collider other){
        if(other.tag != "Ground"){
            return;
        }
        else{
            myAnimator.SetBool("falling",false);
            canJump = true;
            
        }
    }

    void OnTriggerStay(Collider other){
        if(other.tag != "Ground"){
            return;
        }
        else{
            myAnimator.SetBool("falling",false);
            canJump = true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag != "Ground"){
            return;
        }
        else{
            myAnimator.SetBool("falling",true);
            canJump = false;    
        }
    }


    public void resetJump(){
        canJump = true;
    }

    public void cantJump(){
        canJump = false;
    }
}
}