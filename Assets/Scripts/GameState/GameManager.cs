using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace GameState
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class GameManager : MonoBehaviour
    {
        protected readonly List<PlayerManager> Players = new List<PlayerManager>();
        public List<LayerMask> playerLayers;

        public Checkpoint startPoint;

        public int lives = 2;
        public TextMeshProUGUI lifeCount;

        private LobbyController _lobbyController;
        public float startDelay = 1f;

        private AudioListener _listener;
        public bool followPlayers = true; //use this to follow players when attached to audio listener

        private PlayerInputManager _inputManager;
        public GameTimer timer;
        public string gameOverScene = "GameOver";
        public float gameOverDelay;
        
        private void Awake()
        {
            _inputManager = GetComponent<PlayerInputManager>();
            UpdateLives();

            _listener = GetComponentInChildren<AudioListener>();
            _lobbyController = GetComponentInChildren<LobbyController>();
            
            if(!timer) timer = GetComponent<GameTimer>();
            if(timer) timer.outOfTime.AddListener(OnOutOfTime);
        }

        private void OnEnable()
        {
            _inputManager.onPlayerJoined += AddPlayer;
        }

        private void OnDisable()
        {
            _inputManager.onPlayerJoined -= AddPlayer;
        }

        private void Update()
        {
            if(!_listener || !followPlayers || Players.Count < 1) return;
            
            //Set location to between players. Useful when audio listener is attached to this object.
            Vector3 totalPos = Players.Where(player => player.Alive && player.isActiveAndEnabled).Aggregate(Vector3.zero,
                (current, player) => current + player.GetLocation());

            _listener.transform.position = totalPos / Players.Count;
        }

        private void UpdateLives()
        {
            if(lifeCount) lifeCount.text = "Lives: " + lives;
        }

        private void AddPlayer(PlayerInput playerInput)
        {
            PlayerManager player = playerInput.GetComponentInChildren<PlayerManager>();
            player.id = Players.Count; //id matches list index

            Players.Add(player);

            player.onDeath.AddListener(OnDeath);

            player.currCheckPoint = startPoint;
            startPoint.AddPlayer(player.id);

            int layerToAdd = (int)Mathf.Log(playerLayers[Players.Count - 1].value, 2);

            playerInput.GetComponentInChildren<CinemachineFreeLook>().gameObject.layer = layerToAdd;
            player.gameObject.layer = layerToAdd;
            playerInput.GetComponentInChildren<Camera>().cullingMask |= 1 << layerToAdd; //bitwise voodoo
            playerInput.GetComponentInChildren<CameraInputHandler>().horizontal = playerInput.actions.FindAction("Look");

            player.Initialize();
            player.Spawn();
            
            if(_lobbyController) _lobbyController.AddPlayer(player.id);
            player.SetAlive(false); // Keep deactivated until all players have joined.

            if(Players.Count >= _inputManager.maxPlayerCount) StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(startDelay);
            
            foreach (PlayerManager player in Players) player.SetAlive(true);
            if(_lobbyController) _lobbyController.StartGame();
            if(timer) timer.running = true;
        }

        protected virtual void OnDeath(int id)
        {
            lives--;
            UpdateLives();

            if(lives < 1) StartCoroutine(OnGameOver());
            else Players[id].Spawn();
        }

        private void OnOutOfTime()
        {
            lives = 1;
            UpdateLives();
        }

        protected virtual IEnumerator OnGameOver()
        {
            yield return new WaitForSeconds(gameOverDelay);
            SceneManager.LoadScene(gameOverScene);
        }
    }
}