using System.Collections;
using UnityEngine;

namespace GameState
{
    public class MultiGoalManager : GoalManager
    {
        private GameManager _gameManager;

        protected override void Start()
        {
            _gameManager = FindAnyObjectByType<GameManager>();
            if(!_gameManager) Debug.LogError("MultiGoalManager: No GameManager Found");
            SaveLevel = false;
            base.Start();
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if(_gameManager.lives < 2) numPlayers = 1;

            base.OnTriggerEnter(other);
        }

        protected override IEnumerator EndLevel()
        {
            int ending = 0;
            if(numPlayers > 1) ending = 3;
            else
                foreach (PlayerManager player in PlayersInside)
                    ending = player.id == 0 ? 1 : 2;
            
            PlayerPrefs.SetInt("Ending", ending);
            return base.EndLevel();
        }
    }
}