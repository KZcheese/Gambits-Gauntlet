using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActiveRagdoll
{
    public class MultiplayerShowSkinnedMeshScript : MonoBehaviour
{
    public MeshRenderer[] baseLimbRenderers;

    public SkinnedMeshRenderer skinnedMeshRenderer;

    public bool isUsingSkinnedMesh = true;


    //Adding Here
    public MultiplayerPlayerController playerController;
    public MultiplayerPlayerController.MovementType movementType;

    // Start is called before the first frame update
    void Start()
    {
        movementType = playerController.movementType;
    }

    // Update is called once per frame
    void Update()
    {
        if (movementType == MultiplayerPlayerController.MovementType.keyboard)
        {
            if (Input.GetKeyDown("x"))
            {
                if (isUsingSkinnedMesh)
                {
                    isUsingSkinnedMesh = false;
                    skinnedMeshRenderer.enabled = false;
                    foreach (MeshRenderer meshRenderer in baseLimbRenderers)
                    {
                        meshRenderer.enabled = true;
                    }
                }
                else
                {
                    isUsingSkinnedMesh = true;
                    skinnedMeshRenderer.enabled = true;
                    foreach (MeshRenderer meshRenderer in baseLimbRenderers)
                    {
                        meshRenderer.enabled = false;
                    }
                }
            }
        }
        
    }
}
    
}
