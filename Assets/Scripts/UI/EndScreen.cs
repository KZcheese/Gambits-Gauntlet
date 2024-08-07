using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class EndScreen : UIController
    {
        public GameObject defaultSelected;
        public GameObject bgLevel;

        public GameObject[] endings;
        private int _currEnding;

        private EventSystem _eventSystem;

        // Start is called before the first frame update
        private void Start()
        {
            _eventSystem = FindAnyObjectByType<EventSystem>();
            if(_eventSystem) _eventSystem.SetSelectedGameObject(defaultSelected);

            _currEnding = PlayerPrefs.GetInt("Ending", 0);
            for (int i = 0; i < endings.Length; i++)
                endings[i].SetActive(i == _currEnding);

            bgLevel.SetActive(_currEnding != 0);
        }
    }
}