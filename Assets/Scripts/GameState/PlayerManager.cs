using PrototypeRagdoll;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace GameState
{
    public class PlayerManager : MonoBehaviour
    {
        [HideInInspector] public int id;

        [HideInInspector] public Checkpoint currCheckPoint;

        public bool Alive { get; private set; } = true;

        private PrototypePlayerController _playerController;
        private PrototypePhysicsJointController _playerRagdoll;

        private PauseScreen _pauseScreen;

        public GameObject p1Model;
        public GameObject p2Model;

        public UnityEvent<int> onDeath;

        private Canvas _blackScreen;

        public void Initialize()
        {
            _blackScreen = GetComponentInChildren<Canvas>(true);
            _pauseScreen = FindAnyObjectByType<PauseScreen>();

            _playerController = GetComponentInChildren<PrototypePlayerController>();
            _playerRagdoll = GetComponentInChildren<PrototypePhysicsJointController>();

            _playerController.onTriggerEnter.AddListener(OnTriggerEnter);
            _playerController.onTriggerExit.AddListener(OnTriggerExit);

            // prevent collisions with self
            int playerLayer = gameObject.layer;
            Collider[] colliders = GetComponentsInChildren<Collider>(true);
            foreach (Collider c in colliders)
            {
                c.gameObject.layer = playerLayer;
                c.excludeLayers |= 1 << playerLayer;
            }

            p1Model.SetActive(id == 0);
            p2Model.SetActive(id == 1);
        }

        public void TogglePause(InputAction.CallbackContext context)
        {
            if(context.performed && _pauseScreen) _pauseScreen.SetPause(!PauseScreen.GamePaused);
        }

        public Vector3 GetLocation()
        {
            return Alive ? _playerController.transform.position : Vector3.zero;
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Checkpoint"))
            {
                Checkpoint checkpoint = other.GetComponent<Checkpoint>();
                if(!checkpoint || checkpoint.Equals(currCheckPoint)) return;

                currCheckPoint.RemovePlayer(id);
                checkpoint.AddPlayer(id);
                currCheckPoint = checkpoint;
            }
            else if(other.CompareTag("Death")) onDeath.Invoke(id);
            else if(other.CompareTag("Moving"))
            {
                transform.SetParent(other.gameObject.transform);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if(other.CompareTag("Moving")) transform.SetParent(null);
        }

        public void Teleport(Vector3 pos)
        {
            //reset parent position to compensate for moving platforms
            transform.position = Vector3.zero;

            // move both children at once to prevent glitches
            _playerController.gameObject.transform.position = pos;
            _playerRagdoll.gameObject.transform.position = pos;
        }

        public void Spawn()
        {
            Teleport(currCheckPoint.GetSpawn(id).position);
        }

        public void SetAlive(bool alive)
        {
            Alive = alive;
            _playerController.gameObject.SetActive(alive);
            _playerRagdoll.gameObject.SetActive(alive);
            if(_blackScreen) _blackScreen.gameObject.SetActive(!alive);
        }
    }
}