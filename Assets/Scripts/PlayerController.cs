using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Tilt;
    public float XMin;
    public float XMax;
    public float ZMin;
    public float ZMax;
    public GameObject Shot;
    public GameObject Shield;

    public Transform ShotSpawn1;
    public Transform[] ShotSpawn2;
    public Transform[] ShotSpawn3;
    public Transform[] ShotSpawn4;

    private Rigidbody _rigidbody;
    private float _nextFire;
    private float _fireRate;
    private PlayerStats _playerStats;

    public void SetShieldStatus(bool status)
    {
        Shield.SetActive(status);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;

            switch (_playerStats.NumberOfGuns)
            {
                case 1:
                    Instantiate(Shot, ShotSpawn1.position, ShotSpawn1.rotation);
                    break;

                case 2:
                    Instantiate(Shot, ShotSpawn2[0].position, ShotSpawn2[0].rotation);
                    Instantiate(Shot, ShotSpawn2[1].position, ShotSpawn2[1].rotation);
                    break;

                case 3:
                    Instantiate(Shot, ShotSpawn3[0].position, ShotSpawn3[0].rotation);
                    Instantiate(Shot, ShotSpawn3[1].position, ShotSpawn3[1].rotation);
                    Instantiate(Shot, ShotSpawn3[2].position, ShotSpawn3[2].rotation);
                    break;

                case 4:
                    Instantiate(Shot, ShotSpawn4[0].position, ShotSpawn4[0].rotation);
                    Instantiate(Shot, ShotSpawn4[1].position, ShotSpawn4[1].rotation);
                    Instantiate(Shot, ShotSpawn4[2].position, ShotSpawn4[2].rotation);
                    Instantiate(Shot, ShotSpawn4[3].position, ShotSpawn4[3].rotation);
                    break;
            }
        }
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerStats = JsonUtility.FromJson<PlayerStats>(PlayerPrefs.GetString("PlayerStats"));
        _fireRate = _playerStats.FireRate;

    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        int speed = 25;
#else
        float moveHorizontal = Input.acceleration.x;
        float moveVertical = 0;
        int speed = 70;
#endif

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        _rigidbody.velocity = movement * speed;

        _rigidbody.position = new Vector3(Mathf.Clamp(_rigidbody.position.x, XMin, XMax), 0.0f, Mathf.Clamp(_rigidbody.position.z, ZMin, ZMax));

        _rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, _rigidbody.velocity.x * -Tilt);
    }
}