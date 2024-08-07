using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    //Each lim needs to know which limb they are going to follow
    public Transform targetLimb;
    public bool mirror;
    //Configugrable join reference
    ConfigurableJoint cj;



    // Start is called before the first frame update
    void Start()
    {
        cj = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mirror)
        {
            cj.targetRotation = targetLimb.rotation;
        }
        else
        {
            cj.targetRotation = Quaternion.Inverse(targetLimb.rotation);
        }
        
    }
}
