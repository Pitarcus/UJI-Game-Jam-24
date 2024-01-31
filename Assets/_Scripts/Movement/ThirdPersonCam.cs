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

    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

  
    void Update()
    {
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);

        _orientation.forward = viewDir;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 inputDir = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        _playerGeo.forward =Vector3.Slerp(_playerGeo.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}
