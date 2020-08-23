using UnityEngine;

public class MissleMove : MonoBehaviour
{
    public float Speed;

    void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }
}
