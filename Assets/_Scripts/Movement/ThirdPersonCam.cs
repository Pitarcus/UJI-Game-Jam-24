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
    [SerializeField] private float[] radiusPerLevel;    // Starting from the first upgrade level
    [SerializeField] private float[] YOffsetPerLevel;    // Starting from the first upgrade level
    [SerializeField] private float transitionTime;
    [Space]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;

    private Transform _mainCameraTransform;
    private FollowPosition _followPosition;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _mainCameraTransform = Camera.main.transform;
        _followPosition = GetComponent<FollowPosition>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _mainCameraTransform = Camera.main.transform;
    }

    public void StopScript()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeScript()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector3 viewDir = _player.position - new Vector3(_mainCameraTransform.position.x, _player.position.y, _mainCameraTransform.position.z);

        _orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

       // Vector3 inputDir = _orientation.forward * verticalInput + _orientation.right * horizontalInput;

        _playerGeo.forward = Vector3.Slerp(_playerGeo.forward, viewDir.normalized, Time.deltaTime * rotationSpeed);

        Vector3 newPositionVector = _rb.transform.position - _orientation.forward * radius;

        _playerGeo.position = Vector3.Lerp(_playerGeo.position,
            new Vector3(newPositionVector.x, _playerGeo.position.y, newPositionVector.z),
            movementSpeed);
    }

    public void IncreasePositionRadius(int currentLevel)
    {
        radius = radiusPerLevel[currentLevel - 1];
        DOVirtual.Float(radius, radiusPerLevel[currentLevel - 1], transitionTime, RadiusSetter);
    }

    private void RadiusSetter(float value)
    {
        radius = value;
    }


    public void OffsetPlayerY(int currentLevel)
    {
        _followPosition.IncreaseYOffset(YOffsetPerLevel[currentLevel-1]);
    }
}
