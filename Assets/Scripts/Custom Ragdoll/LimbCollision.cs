using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCollision : MonoBehaviour
{

    public PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        playerController.isGrounded = true;
    }


}
