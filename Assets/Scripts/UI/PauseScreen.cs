using System.Linq;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseScreen : StartScreen
{
    public static bool GamePaused;
    public GameObject pauseMenu;
    private Menu _defaultMenu;

    protected override void Start()
    {
        GetMenus();
        _defaultMenu = activeMenu;
    }
    
    public void SetPause(bool paused)
    {
        GamePaused = paused;
        pauseMenu.SetActive(paused);
        Time.timeScale = paused ? 0 : 1;
        if(paused) ChangeMenu(_defaultMenu);

        PlayerInput[] inputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None);

        // check if any active players are using a mouse (pointer type device)
        bool mouseActive = inputs.Any(input => input.devices
            .Select(device => device.GetType())
            .Any(deviceType => deviceType.IsAssignableFrom(typeof(Pointer)) || typeof(Pointer).IsAssignableFrom(deviceType)));

        if(!mouseActive) return;
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        // Mouse.current.WarpCursorPosition(Vector2.one); // move mouse out of the way so it doesn't start in the center
    }

    public override void ReloadScene()
    {
        SetPause(false);
        base.ReloadScene();
    }


    public override void ChangeScene(string sceneName)
    {
        SetPause(false);
        Cursor.lockState = CursorLockMode.None;
        base.ChangeScene(sceneName);
    }
}