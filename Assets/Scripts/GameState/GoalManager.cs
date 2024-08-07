using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class GoalManager : MonoBehaviour
    {
        public string nextLevel;
        protected bool SaveLevel = true;

        public int numPlayers;
        public float endLevelDelay;

        //hashsets prevent duplicates
        protected readonly HashSet<PlayerManager> PlayersInside = new HashSet<PlayerManager>();
        private Material _mat;

        private Color _emptyColor;
        public Color halfColor;
        public Color fullColor;
        private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

        protected virtual void Start()
        {
            _mat = GetComponent<Renderer>().material;
            if(!_mat) return;
            _emptyColor = _mat.color;
            _mat.SetColor(EmissionColor, _emptyColor / 2);

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = GetPlayerObject(other.gameObject);
            if(!player) return;

            PlayersInside.Add(player);
            UpdateColor();

            if(PlayersInside.Count >= numPlayers) StartCoroutine(EndLevel());
        }

        private void OnTriggerExit(Collider other)
        {
            PlayerManager player = GetPlayerObject(other.gameObject);
            if(!player) return;

            PlayersInside.Remove(player);
            UpdateColor();
        }

        private static PlayerManager GetPlayerObject(GameObject colliderObject)
        {
            return colliderObject.CompareTag("Player") ? colliderObject.GetComponentInParent<PlayerManager>() : null;
        }

        private void UpdateColor()
        {
            if(!_mat) return;

            if(PlayersInside.Count < numPlayers / 2) SetColor(_emptyColor);
            else if(PlayersInside.Count < numPlayers) SetColor(halfColor);
            else SetColor(fullColor);
        }

        private void SetColor(Color color)
        {
            _mat.color = color;
            _mat.SetColor(EmissionColor, color / 2);
        }

        protected virtual IEnumerator EndLevel()
        {
            if(SaveLevel) PlayerPrefs.SetString("CurrentLevel", nextLevel);

            yield return new WaitForSeconds(endLevelDelay);
            SceneManager.LoadScene(nextLevel);
        }
    }
}