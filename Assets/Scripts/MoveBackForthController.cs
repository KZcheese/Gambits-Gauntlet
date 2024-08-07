using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interactables;

public class MoveBackForthController : Interactable
{

    public float moveSpeed;

    public Transform target;
    public Transform target2;

    public bool destination1 = true;

    private Vector3 _originalPosition;

    // Start is called before the first frame update
    private void Start()
    {
        _originalPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (activated == false)
        {
            return;
        }

        Vector3 destination = target.position;

        if (destination1 == false)
        {
            destination = target2.position;
        }

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            Debug.Log("Passed the Destination: " + destination);
            if (destination == target.position)
            {
                destination = target2.position;
                destination1 = false;
                return;
            }
            else if (destination == target2.position)
            {
                destination = target.position;
                destination1 = true;
                return;
            }
        }

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);

        
    }
}
