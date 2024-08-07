using ActiveRagdoll;
using Interactables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PrototypeRagdoll
{
    [RequireComponent(typeof(Rigidbody))]
    public class PrototypeGrabScript : MonoBehaviour
    {
        //Adding Here
        private Rigidbody _rigidBody;
        private bool _isHolding;
        private GameObject _heldObject;

        public float grabStrength = 100f;
        private ConfigurableJoint _grabJoint;
        public float throwMultiplier = 2f;

        public bool isLeftArm;

        public Animator myAnimator;
        public Transform gunHolder;

        public AudioSource audioSource;
        public AudioClip grabNoise, releaseNoise;
        private static readonly int HoldingLeft = Animator.StringToHash("HoldingLeft");
        private static readonly int Holding = Animator.StringToHash("Holding");
        private static readonly int HoldingRight = Animator.StringToHash("HoldingRight");

        public PrototypePlayerController playerController;
        private bool _grabLeft;
        private bool _grabRight;

        public void GrabLeft(InputAction.CallbackContext context)
        {
            _grabLeft = context.performed;
        }

        public void GrabRight(InputAction.CallbackContext context)
        {
            _grabRight = context.performed;
        }

        public void Shoot(InputAction.CallbackContext context)
        {
            if(!context.performed || !_isHolding) return;

            GunScript gun = _heldObject.GetComponent<GunScript>();
            if(gun) gun.Shoot();
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void Update()
        {
            if(isLeftArm)
            {
                if(!_grabLeft)
                {
                    myAnimator.SetBool(HoldingLeft, false);
                    myAnimator.SetBool(Holding, false);
                    DropObject();
                    _isHolding = false;
                }
                else
                    myAnimator.SetBool(_grabRight ? Holding : HoldingLeft, true);

            }
            else
            {
                if(_grabRight)
                    myAnimator.SetBool(_grabLeft ? Holding : HoldingRight, true);
                else
                {
                    myAnimator.SetBool(HoldingRight, false);
                    myAnimator.SetBool(Holding, false);
                    DropObject();
                    _isHolding = false;
                }
            }

            if(_grabRight) Cursor.lockState = CursorLockMode.Locked;
        }

        private void DropObject()
        {
            if(!_heldObject) return;

            Destroy(_grabJoint);
            _heldObject.GetComponent<Rigidbody>().velocity *= throwMultiplier;

            if(_heldObject.CompareTag("Interactable"))
            {
                ButtonController button = _heldObject.GetComponent<ButtonController>();
                if(button.mode.Equals(ButtonController.ButtonMode.Hold)) button.SetActivate(false);
            }

            if(_heldObject.CompareTag("Swing"))
            {
                if(isLeftArm) playerController.leftSwing = false;
                else playerController.rightSwing = false;
            }

            _heldObject = null;
            audioSource.PlayOneShot(releaseNoise);
        }

        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log("TRIGGERED");

            if(_isHolding || (!_grabLeft && !_grabRight)) return;

            GameObject grabbedObject = other.gameObject;
            if(!grabbedObject) return;

            Rigidbody grabbedRigidbody = grabbedObject.GetComponent<Rigidbody>();
            if(!grabbedRigidbody) return;

            if(grabbedObject.CompareTag("Player") ||
               !(grabbedObject.CompareTag("grabbable") ||
                 grabbedObject.CompareTag("gun") ||
                 grabbedObject.CompareTag("Swing") ||
                 grabbedObject.CompareTag("Interactable")))
                return;

            if((!isLeftArm || !_grabLeft) && (isLeftArm || !_grabRight)) return;

            if(grabbedObject.CompareTag("gun"))
            {
                grabbedObject.transform.position = gunHolder.position;
                grabbedObject.transform.rotation = gunHolder.rotation;
            }

            if(grabbedObject.CompareTag("Interactable"))
            {
                Debug.Log("Holding Button");
                ButtonController button = grabbedObject.GetComponent<ButtonController>();
                if(button.mode.Equals(ButtonController.ButtonMode.Hold)) button.SetActivate(true);
                else button.Toggle();
            }

            if(grabbedObject.CompareTag("Swing"))
            {
                if(isLeftArm) playerController.leftSwing = true;
                else playerController.rightSwing = true;
            }

            myAnimator.SetBool(isLeftArm ? HoldingLeft : HoldingRight, true);

            _isHolding = true;
            _heldObject = grabbedObject;
            _grabJoint = _heldObject.AddComponent(typeof(ConfigurableJoint)) as ConfigurableJoint;
            _grabJoint!.connectedBody = _rigidBody;

            JointDrive jointDrive = _grabJoint.angularXDrive;
            jointDrive.positionSpring = grabStrength;

            _grabJoint.xMotion = ConfigurableJointMotion.Locked;
            _grabJoint.yMotion = ConfigurableJointMotion.Locked;
            _grabJoint.zMotion = ConfigurableJointMotion.Locked;

            _grabJoint.angularXMotion = ConfigurableJointMotion.Locked;
            _grabJoint.angularYMotion = ConfigurableJointMotion.Locked;
            _grabJoint.angularZMotion = ConfigurableJointMotion.Locked;

            _grabJoint.breakForce = 2000f;

            _grabJoint.angularXDrive = jointDrive;
            _grabJoint.angularYZDrive = jointDrive;

            _grabJoint.anchor = new Vector3(0, 0, 0);
            audioSource.PlayOneShot(grabNoise);
        }
    }
}