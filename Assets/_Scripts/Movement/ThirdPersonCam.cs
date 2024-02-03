using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("references")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerGeo;
    [SerializeField] private Rigidbody _rb;

    [Header("RollingBallGameParameters")]
    [SerializeField] private float radius;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;

    private Transform _mainCameraTransform;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _mainCameraTransform = Camera.main.transform;
    }

  
    void Update()
    {
        Vector3 viewDir = _player.position - new Vector3(_mainCameraTransform.position.x, _player.position.y, _mainCameraTransform.position.z);

        _orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

       // Vector3 inputDir = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        _playerGeo.forward = Vector3.Slerp(_playerGeo.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);
        _playerGeo.position = Vector3.Lerp(_playerGeo.position, _rb.transform.position - _orientation.forward * radius, movementSpeed);
    }
}
