using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float Lifetime;

    void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}