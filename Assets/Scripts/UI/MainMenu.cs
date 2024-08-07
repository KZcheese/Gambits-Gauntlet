using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenu : Menu
    {
        public TMP_Text startText;
        public string startLevel;
        private string _currLevel;

        private void Start()
        {
            _currLevel = PlayerPrefs.GetString("CurrentLevel", startLevel);
            startText.text = _currLevel.Equals(startLevel) ? "Start" : "Continue";
        }

        public void LoadCurrentLevel()
        {
            SceneManager.LoadScene(_currLevel);
        }
    }
}