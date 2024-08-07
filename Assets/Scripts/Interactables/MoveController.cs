using UnityEngine;

namespace Interactables
{
    public class MoveController : Interactable
    {
        public float moveSpeed;
        public float rotationSpeed;

        public Transform targetPosition;
        public Quaternion targetRotation;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private void Start()
        {
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        private void Update()
        {
            // Handle position
            Vector3 destination = activated ? targetPosition.position : _originalPosition;
            if (Vector3.Distance(transform.position, destination) > 0.01f)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
            }

            // Handle rotation
            Quaternion targetRot = activated ? targetRotation : _originalRotation;
            if (Quaternion.Angle(transform.rotation, targetRot) > 0.01f)
            {
                float rotStep = rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotStep);
            }
        }
    }
}
