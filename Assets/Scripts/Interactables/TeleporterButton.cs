using Interactables;
using UnityEngine;

public class TeleporterButton : ButtonController
{
    protected override void Start()
    {
        interactables.AddRange(FindObjectsByType<Teleporter>(FindObjectsSortMode.None));
        base.Start();
    }
}