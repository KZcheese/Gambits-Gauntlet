using System.Collections;
using UnityEngine;

namespace GameState
{
    public class SuddenDeathGameManager : GameManager
    {
        protected override void OnDeath(int id)
        {
            Players[id].SetAlive(false);
            base.OnDeath(id);
        }

        protected override IEnumerator OnGameOver()
        {
            PlayerPrefs.SetInt("Ending", 0);
            return base.OnGameOver();
        }
    }
}