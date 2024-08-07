using System;
using Interactables;
using UnityEngine;

public class BetrayalPadScript : Interactable
{
    [Header("Movable Objects")] public GameObject movablePath;
    public float pathSpeed;
    public Transform movePathPosition;
    public Interactable door;

    [Header("Invisible Wall for Chamber")] public GameObject invisibleWall;

    // I don't think we need this since there's no way for the players to get on the wrong pad.
    [Header("Player1: 8, Player2: 9")] public int wantedPlayerLayer;

    [Header("Check Players")] public bool playerIsOn;
    public bool playerIsLockedInside;
    public bool playerGotSaved;

    [Header("Other Player's Pad")] public GameObject otherPad;

    [Header("Elevator Stuff")] public MoveController elevator;
    public Transform savedPosition;
    public Transform killedPosition;

    // Update is called once per frame
    private void Update()
    {
        if(playerIsOn && !playerIsLockedInside)
        {
            Vector3 destination = movePathPosition.position;

            if(Vector3.Distance(movablePath.transform.position, destination) < 0.01f) return;

            float step = pathSpeed * Time.deltaTime;
            movablePath.transform.position = Vector3.MoveTowards(movablePath.transform.position, destination, step);

            //Close the door
            door.activated = false;

            //Set Wall for no escape in chamber room
            Invoke(nameof(LockPlayerInChamber), 2.0f);
        }
        else if(playerIsLockedInside && otherPad.GetComponent<BetrayalPadScript>().playerIsLockedInside)
        {
            elevator.targetPosition = playerGotSaved ? savedPosition : killedPosition;
            Invoke(nameof(ActivateElevator), 2.0f);
        }

        playerGotSaved = message.ToLowerInvariant() switch
        {
            "red" => false,
            "green" => true,
            _ => playerGotSaved
        };
    }

    private void ActivateElevator()
    {
        elevator.activated = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerIsOn = other.gameObject.layer == wantedPlayerLayer;
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == wantedPlayerLayer) playerIsOn = false;
    }

    private void LockPlayerInChamber()
    {
        invisibleWall.SetActive(true);
        playerIsLockedInside = true;
    }
}