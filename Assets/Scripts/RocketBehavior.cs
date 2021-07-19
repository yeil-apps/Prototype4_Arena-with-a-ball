using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBehavior : MonoBehaviour
{
    private Transform _target;
    private float _speed = 15f;
    private bool _homing;

    [SerializeField] private float _rocketStrange = 15f;
    [SerializeField] private float _aliveTimer = 5f;
    // Update is called once per frame
    void Update()
    {
        if (_homing && _target != null)
        {
            Vector3 moveDirection = (_target.position - transform.position).normalized;
            transform.position += moveDirection * _speed * Time.deltaTime;
            transform.LookAt(_target);
        }   else
        {
            Destroy(transform.gameObject);
        }
    }

    public void Fire(Transform newTarget)
    {
        _target = newTarget;
        _homing = true;
        Destroy(transform.gameObject, _aliveTimer);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_target != null)
        {
            if (collision.gameObject.CompareTag(_target.tag))
            {
                Rigidbody targetRigidBody = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 away = -collision.contacts[0].normal;
                targetRigidBody.AddForce(away * _rocketStrange, ForceMode.Impulse);
                Destroy(transform.gameObject);
            }
        }
    }
}
