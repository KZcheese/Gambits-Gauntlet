using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public Vector3 axis = Vector3.up;

    private void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}