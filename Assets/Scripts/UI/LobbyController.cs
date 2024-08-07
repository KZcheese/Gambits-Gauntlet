using TMPro;
using UnityEngine;

namespace UI
{
    public class LobbyController : MonoBehaviour
    {
        public GameObject lobbyCamera;
        public TMP_Text[] playerStatuses;
    
        public void StartGame()
        {
            lobbyCamera.SetActive(false);
            gameObject.SetActive(false);
        }
    
        public void AddPlayer(int id)
        {
            playerStatuses[id].text = "Ready!";
        }
    }
}
