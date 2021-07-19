    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 2f;
    private float _horizontalInput;

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, _horizontalInput * _rotationSpeed * Time.deltaTime);
    }
}
