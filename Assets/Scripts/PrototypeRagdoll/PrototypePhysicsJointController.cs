using UnityEngine;
using UnityEngine.InputSystem;

namespace PrototypeRagdoll
{
    [RequireComponent(typeof(ConfigurableJoint))]
    public class PrototypePhysicsJointController : MonoBehaviour
    {
        private ConfigurableJoint _myJoint;
        JointDrive baseDriveAngX, baseDriveAngYZ, basePositionDrive;
        public bool isLimp;
        public float RagdollForce = 3f;
        public float RagdollDuration = 1.5f;

        public PrototypeActiveRagdollController ragdollController;
        public bool isBody;
        public bool isLeg;

        public AudioSource audioSource;
        public AudioClip impactSound1;
        public AudioClip impactSound2;

        public PrototypePlayerController player;

        // Start is called before the first frame update
        private void Start()
        {
            _myJoint = GetComponent<ConfigurableJoint>();
            if(!_myJoint) return;

            baseDriveAngX = _myJoint.angularXDrive;
            baseDriveAngYZ = _myJoint.angularYZDrive;
            basePositionDrive = _myJoint.xDrive;
        }

        public void Limp(InputAction.CallbackContext context)
        {
            if(!context.performed) return;
            if(isLimp) ResetLimbStrength();
            else
            {
                GoLimp();
                if(isBody) SetPositionStrength(0);
            }
        }

        public void SetLimbStrength(float strength)
        {
            if(!_myJoint) return;

            JointDrive newDrive = _myJoint.angularXDrive;
            newDrive.positionSpring = strength;
            newDrive.positionDamper = baseDriveAngX.positionDamper;

            _myJoint.angularXDrive = newDrive;
            _myJoint.angularYZDrive = newDrive;
        }

        public void SetPositionStrength(float strength)
        {
            if(!_myJoint) return;

            JointDrive newDrive = _myJoint.xDrive;
            newDrive.positionSpring = strength;
            newDrive.positionDamper = _myJoint.xDrive.positionDamper;

            _myJoint.xDrive = newDrive;
            _myJoint.yDrive = newDrive;
            _myJoint.zDrive = newDrive;
        }

        public void ResetPositionStrength()
        {
            if(!_myJoint) return;

            _myJoint.xDrive = basePositionDrive;
            _myJoint.yDrive = basePositionDrive;
            _myJoint.zDrive = basePositionDrive;

            player.isLimp = false;
            player.gameObject.transform.position = transform.position;
            player.gameObject.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
        }

        public void ResetLimbStrength()
        {
            _myJoint.angularXDrive = baseDriveAngX;
            _myJoint.angularYZDrive = baseDriveAngYZ;

            isLimp = false;
            if(isBody) ResetPositionStrength();
        }

        public void GoLimp()
        {
            SetLimbStrength(0);
            isLimp = true;
        }

        public void GoLimpForSeconds(float seconds)
        {
            SetLimbStrength(0);
            isLimp = true;
            Invoke(nameof(ResetLimbStrength), seconds);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!(collision.relativeVelocity.magnitude >= RagdollForce)) return;

            if(isBody)
            {
                ragdollController.GoLimpForSeconds(1.5f);
                player.isLimp = true;
            }
            else if(isLeg)
            {
                ragdollController.GoLimpForSeconds(.5f);
                player.isLimp = true;
            }

            // Disable stupid thump sound until we find a better one
            // if(audioSource && !audioSource.isPlaying)
            // {
            //     int i = (int)Random.Range(0, 5);
            //     audioSource.PlayOneShot(i < 3 ? impactSound1 : impactSound2);
            // }

            GoLimp();
            Invoke(nameof(ResetLimbStrength), RagdollDuration);
        }
    }
}