using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace PrototypeRagdoll
{
    [RequireComponent(typeof(Rigidbody))]
    public class PrototypePlayerController : MonoBehaviour
    {
        private Rigidbody _rigidBody;
        public Animator myAnimator;
        public float jumpSpeed = 400f;
        private bool _canJump = true;

        public Camera cam;

        private float _currentSpeed;

        // public float turnspeed = 10f;
        public float walkSpeed = 2500f;
        public float sprintSpeed = 4000f;
        public float swingMultiplier = 2f;

        public AudioSource audioSource;
        public AudioClip jumpNoise;

        // public ConfigurableJoint capsuleJoint;
        // public GameObject capsuleObject;
        public bool isLimp;

        // public PrototypePhysicsJointController capsuleController;
        // public float sprintAnimModifier = 1.4f;
        private static readonly int Moving = Animator.StringToHash("moving");
        private static readonly int Raising = Animator.StringToHash("raising");
        private static readonly int Falling = Animator.StringToHash("falling");

        private Vector2 _moveInput = Vector2.zero;
        private bool _isGrabbing;

        public UnityEvent<Collider> onTriggerEnter;
        public UnityEvent<Collider> onTriggerExit;

        [HideInInspector] public bool leftSwing;
        [HideInInspector] public bool rightSwing;

        // Start is called before the first frame update
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            Cursor.lockState = CursorLockMode.Locked;
            _currentSpeed = walkSpeed;
        }

        public void Move(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void Jump(InputAction.CallbackContext context)
        {
            if(!context.performed || !_canJump || !_rigidBody) return;

            _rigidBody.AddForce(transform.up * jumpSpeed);
            myAnimator.SetBool(Falling, true);
            _canJump = false;
            audioSource.PlayOneShot(jumpNoise);
        }

        public void Run(InputAction.CallbackContext context)
        {
            _currentSpeed = context.performed ? sprintSpeed : walkSpeed;
        }

        public void Grab(InputAction.CallbackContext context)
        {
            _isGrabbing = context.performed;
        }

        public void RaiseArms(InputAction.CallbackContext context)
        {
            myAnimator.SetBool(Raising, context.performed);
        }

        private void FixedUpdate()
        {
            Quaternion rotation = transform.rotation;
            if(_isGrabbing)
            {
                Quaternion camRotation = cam.transform.rotation;
                rotation = Quaternion.Euler(camRotation.eulerAngles.x, camRotation.eulerAngles.y,
                    rotation.eulerAngles.z);
            }
            else
                rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, rotation.eulerAngles.z);

            if(!isLimp)
            {
                // Convert Vector2 move input to Vector3 move direction based on orientation of the player.
                Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
                if(leftSwing || rightSwing) move *= swingMultiplier;
                _rigidBody.AddForce(move * (_currentSpeed * Time.fixedDeltaTime));

                // If the player is moving and not grabbing, turn the player face the away from the camera.
                if(_rigidBody.velocity.magnitude >= 1f && !_isGrabbing)
                    rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, cam.transform.rotation.eulerAngles.y,
                        rotation.eulerAngles.z);
            }

            transform.rotation = rotation;

            myAnimator.SetBool(Moving, _rigidBody.velocity.magnitude >= 1f);
        }

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(other);

            // if(!other.CompareTag("Ground")) return;

            myAnimator.SetBool(Falling, false);
            _canJump = true;
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke(other);
        }
    }
}