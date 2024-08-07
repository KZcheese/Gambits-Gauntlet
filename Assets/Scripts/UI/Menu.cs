using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public GameObject defaultSelected;

        private EventSystem _eventSystem;

        private void Awake()
        {
            _eventSystem = FindAnyObjectByType<EventSystem>();
        }

        private void OnEnable()
        {
            _eventSystem.SetSelectedGameObject(defaultSelected);
        }
    }
}