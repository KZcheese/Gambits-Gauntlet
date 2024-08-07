using GameState;
using UnityEngine;

namespace Interactables
{
    public class Teleporter : Interactable
    {
        public Transform destination;
        private MoveController _moveController; // To reference the MoveController

        private bool _isReadyToTeleport; // Flag to check if teleport is ready

        private void Start()
        {
            _moveController = GetComponentInParent<MoveController>(); // Find the MoveController in the parent
        }

        private void Update()
        {
            // Check if the teleporter is activated and ready
            // should this be necessary? that's a sign that activated isn't being kept track of --Kevin
            if(activated && !_isReadyToTeleport) activated = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            _isReadyToTeleport = true;
        }

        private void OnTriggerExit(Collider other)
        {
            _isReadyToTeleport = false;
        }

        private void OnTriggerStay(Collider other)
        {
            if(!destination || !activated || _moveController.activated) return;
            //Debug.Log(other);
            //Debug.Log(isReadyToTeleport);
            if(_isReadyToTeleport && other.CompareTag("Player"))
            {
                PlayerManager player = other.GetComponentInParent<PlayerManager>();
                if(!player) return;

                player.Teleport(destination.position);

                ResetTeleporter();
                //activated = false;
            }
            else if(other.CompareTag("grabbable"))
            {

                Rigidbody grabbableRigidbody = other.GetComponent<Rigidbody>();
                if(!grabbableRigidbody) return;

                // Freeze position and rotation
                grabbableRigidbody.constraints = RigidbodyConstraints.FreezeAll;

                // Teleport the object
                other.transform.position = destination.position;

                // Unfreeze position and rotation
                grabbableRigidbody.constraints = RigidbodyConstraints.None;
                ResetTeleporter();
            }
        }

        private void ResetTeleporter()
        {
            _isReadyToTeleport = false;
            activated = false;
        }
    }
}