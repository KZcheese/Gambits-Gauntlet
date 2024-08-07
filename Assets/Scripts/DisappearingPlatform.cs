using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactables;

public class DisappearingPlatform : Interactable
{
    [SerializeField] private GameObject platform;
    // Start is called before the first frame update
    void Start()
    {
        platform.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        platform.SetActive(activated);
    }
}
