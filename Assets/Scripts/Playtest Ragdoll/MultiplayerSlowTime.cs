using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveRagdoll{
public class MultiplayerSlowTime : MonoBehaviour
{
    float fixedDeltaTime;
    //Adding Here
    public MultiplayerPlayerController playerController;
    public MultiplayerPlayerController.MovementType movementType;

    // Start is called before the first frame update
    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
        movementType = playerController.movementType;
    }

    // Update is called once per frame
    void Update()
    {
        if(movementType == MultiplayerPlayerController.MovementType.keyboard)
        {
            if (Input.GetKeyDown("u") && Time.timeScale >= .2f)
            {
                Time.timeScale = Time.timeScale / 1.25f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }
            else if (Input.GetKeyDown("i") && Time.timeScale < 1f)
            {
                Time.timeScale = Time.timeScale * 1.25f;
                Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            }
        }
        
    }
}
}