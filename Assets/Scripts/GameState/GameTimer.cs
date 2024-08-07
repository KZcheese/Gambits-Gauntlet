using System;
using System.Collections.Generic;
using Interactables;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace GameState
{
    public class GameTimer : MonoBehaviour
    {
        public bool running;
        public int minutes;
        public int seconds;
        public List<Interactable> onTimeEnd;
        public TextMeshProUGUI timerUI;

        private float _totalSeconds;
        private bool _noTime;

        public UnityEvent outOfTime;

        private void Start()
        {
            _totalSeconds = minutes * 60 + seconds;
        }

        private void Update()
        {
            if(!running || _noTime) return;
            _totalSeconds -= Time.deltaTime;

            if(_totalSeconds <= 0) OutOfTime();

            TimeSpan time = TimeSpan.FromSeconds(_totalSeconds);
            if(timerUI) timerUI.text = time.ToString(@"mm\:ss");
        }

        private void OutOfTime()
        {
            outOfTime.Invoke();
            _noTime = true;
            _totalSeconds = 0;
            foreach (Interactable interactable in onTimeEnd)
                interactable.activated = true;
        }
    }
}