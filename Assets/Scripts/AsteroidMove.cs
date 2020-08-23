using UnityEngine;

public class AsteroidMove : MonoBehaviour
{
    public float Speed;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = -transform.forward * Speed;
    }
}
