using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameState
{
    public class Checkpoint : MonoBehaviour
    {
        private Transform[] _spawnPoints;
        private readonly List<int> _players = new List<int>();

        private void Start()
        {
            _spawnPoints = GetComponentsInChildren<Transform>()
                .Where(t => t != transform).ToArray(); // Get children not self
        }

        public void AddPlayer(int player)
        {
            _players.Add(player);
        }

        public void RemovePlayer(int player)
        {
            _players.Remove(player);
        }

        public Transform GetSpawn(int player)
        {
            return _spawnPoints[_players.FindIndex(p => p == player)];
        }
    }
}