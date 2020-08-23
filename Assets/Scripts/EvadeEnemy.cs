using System.Collections;
using UnityEngine;

public class EvadeEnemy : MonoBehaviour
{
    public float Dodge;
    public float Smoothing;
    public float Tilt;
    public Vector2 StartWait;
    public Vector2 ManeuverTime;
    public Vector2 ManeuverWait;
    public float XMin;
    public float XMax;
    public float ZMin;
    public float ZMax;

    private float _currentSpeed;
    private float _targetManeuver;
    private Rigidbody _rigidBody;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _currentSpeed = _rigidBody.velocity.z;
        StartCoroutine(Evade());
    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(StartWait.x, StartWait.y));

        while (true)
        {
            _targetManeuver = Random.Range(1, Dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(ManeuverTime.x, ManeuverTime.y));
            _targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(ManeuverWait.x, ManeuverWait.y));
        }
    }

    void FixedUpdate()
    {
        float newManeuver = Mathf.MoveTowards(_rigidBody.velocity.x, _targetManeuver, Time.deltaTime * Smoothing);
        _rigidBody.velocity = new Vector3(newManeuver, 0.0f, _currentSpeed);
        _rigidBody.position = new Vector3
        (
            Mathf.Clamp(_rigidBody.position.x, XMin, XMax),
            0.0f,
            Mathf.Clamp(_rigidBody.position.z, ZMin, ZMax)
        );

        _rigidBody.rotation = Quaternion.Euler(0.0f, 180f, _rigidBody.velocity.x * -Tilt);
    }
}
