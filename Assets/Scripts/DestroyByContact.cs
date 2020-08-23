using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject Explosion;
    public GameObject PlayerExplosion;
    public int Score;

    private GameController _gameController;

    public void DestroyByUltiy()
    {
        Instantiate(Explosion, transform.position, transform.rotation);
        _gameController.AddScore(Score);
        Destroy(gameObject);
    }

    private void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }

        Instantiate(Explosion, transform.position, transform.rotation);

        if (other.tag == "Player")
        {
            Instantiate(PlayerExplosion, other.transform.position, other.transform.rotation);
            _gameController.GameOver();
        }

        _gameController.AddScore(Score);

        if (other.tag != "Shield")
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
