using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private bool _isBoss = false;
    private int _miniEnemySpawnCount;
    public int MiniEnemySpawnCount
    {
        get { return _miniEnemySpawnCount; }
        set { _miniEnemySpawnCount = value; }
    }

    [SerializeField] private float _spawnInterval = 2f;
    [Space]
    [SerializeField] private float _speed = 300f;

    private float _nextSpawn;


    private SpawnManager _spawnManager;
    private Rigidbody _enemyRb;
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerController>().gameObject;
        _enemyRb = GetComponent<Rigidbody>();
        if (_isBoss)
        {
            _spawnManager = FindObjectOfType<SpawnManager>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookDirection = (_player.transform.position - transform.position).normalized;
        _enemyRb.AddForce(lookDirection * _speed * Time.deltaTime);

        if (transform.position.y < -10)
        {
            Destroy(transform.gameObject);
        }

        if (_isBoss)
        {
            if (Time.time > _nextSpawn)
            {
                _nextSpawn = Time.time + _spawnInterval;
                _spawnManager.SpawnMiniEnemy(_miniEnemySpawnCount);
            }
        }


    }
}
