using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    public GameObject Shot;
    public Transform ShotSpawn;
    public float FireRate;

    void Start()
    {
        InvokeRepeating("Fire", 0, FireRate);
    }

    void Fire()
    {
        Instantiate(Shot, ShotSpawn.position, ShotSpawn.rotation);
    }
}
