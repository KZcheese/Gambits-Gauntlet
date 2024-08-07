using UnityEngine;

namespace PrototypeRagdoll
{
    public class PrototypeActiveRagdollController : MonoBehaviour
    {
        public PrototypePhysicsJointController[] limbs;
        // public bool isLimp = false;

        // public Rigidbody playerBaseBody;

        // public GameObject leftArm, rightArm;

        // public PrototypePlayerController player;
        public GameObject capsuleBody;

        public float maxDistance = 6f;

        // Update is called once per frame
        private void Update()
        {

            if((transform.position - capsuleBody.transform.position).magnitude >= maxDistance)
            {
                transform.position = capsuleBody.transform.position;
            }
        }

        public void SetGrabStrength(float strength)
        {
            limbs[0].SetLimbStrength(strength);
            limbs[1].SetLimbStrength(strength);
            limbs[2].SetLimbStrength(strength);
        }

        public void ResetAllLimbStrength()
        {
            foreach (PrototypePhysicsJointController limb in limbs)
            {
                limb.ResetLimbStrength();
            }
        }

        public void SetBodyPositionStrength(float strength)
        {
            limbs[0].SetPositionStrength(strength);
        }

        private void ResetBodyPositionStrength()
        {
            limbs[0].ResetPositionStrength();
        }

        public void GoLimpForSeconds(float seconds)
        {
            foreach (PrototypePhysicsJointController limb in limbs)
            {
                limb.GoLimp();
            }

            limbs[0].SetPositionStrength(0);

            Invoke(nameof(ResetLimp), seconds);
        }

        public void ResetLimp()
        {
            foreach (PrototypePhysicsJointController limb in limbs)
            {
                limb.ResetLimbStrength();
            }

            ResetBodyPositionStrength();
        }
    }
}