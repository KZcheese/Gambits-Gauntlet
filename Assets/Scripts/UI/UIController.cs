using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        public virtual void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        public virtual void ChangeScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public virtual void QuitGame()
        {
            Application.Quit();
        }
    }
}