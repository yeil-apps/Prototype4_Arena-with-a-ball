using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PowerupType _currentPowerUp = PowerupType.None;

    [SerializeField] private Transform _focalPoint;
    [SerializeField] private GameObject _powerUpIndicator;
    [SerializeField] private GameObject _rocketPrefab;

    [Header("Smash values: ")]
    [SerializeField] private float _hangTime;
    [SerializeField] private float _smashSpeed;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _explosionRadius;

    [Header("Move speed: ")]
    [SerializeField] private float _speed = 500f;

    private float _powerUpStrange = 15f;
    private float _timeBtwSpawnRocket = 0.3f;
    
    private Rigidbody _playerRb;
    private GameObject _tmpRocket;
    private Coroutine _powerupCountdown;

    private float _floorY;
    private bool _onGround = true;
    // Start is called before the first frame update
    void Start()
    {
        _playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        _playerRb.AddForce(_focalPoint.forward * _speed * forwardInput * Time.deltaTime);
        _powerUpIndicator.transform.position = transform.position;

        if (_currentPowerUp == PowerupType.Smash && Input.GetKeyDown(KeyCode.Space) && _onGround)
        {
            StartCoroutine(Smash());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            _currentPowerUp = other.gameObject.GetComponent<PowerUp>().powerupType;

            if (_currentPowerUp == PowerupType.PushBack)
            {
                _powerUpIndicator.gameObject.SetActive(true);
            } 
            else if (_currentPowerUp == PowerupType.Rockets)
            {
                StartCoroutine(LaunchRocketAuto());
            }

            Destroy(other.gameObject);
            if (_powerupCountdown != null)
            {
                StopCoroutine(_powerupCountdown);
            }
            _powerupCountdown = StartCoroutine(PowerUpCountdownRoutine());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && (_currentPowerUp == PowerupType.PushBack))   
        {
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(awayFromPlayer * _powerUpStrange, ForceMode.Impulse);
        }
    }

    private void LaunchRocket()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            _tmpRocket = Instantiate(_rocketPrefab, transform.position + Vector3.up, Quaternion.identity);
            _tmpRocket.GetComponent<RocketBehavior>().Fire(enemy.transform);
        }
    }

    IEnumerator LaunchRocketAuto()
    {
        while (_currentPowerUp == PowerupType.Rockets)
        {
            LaunchRocket();
            yield return new WaitForSeconds(_timeBtwSpawnRocket);
        }
        StopCoroutine(LaunchRocketAuto());
    }

    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        _currentPowerUp = PowerupType.None;
        _powerUpIndicator.gameObject.SetActive(false);
        _onGround = true;
    }
    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        _floorY = transform.position.y;
        float jumpTime = Time.time + _hangTime;
        _onGround = false;

        while (Time.time < jumpTime)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, _smashSpeed);
            yield return null;
        }

        while (transform.position.y > _floorY)
        {
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, -_smashSpeed * 2);
            yield return null;
        }

        _onGround = true;
        for (int i = 0; i<enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 0f, ForceMode.Impulse);
            }
        }      
    }
}
