using System;
using UnityEngine;

public class InspectionCamera : MonoBehaviour
{
    [NonSerialized] public InspectableObject inspectableObject;
    [SerializeField] private Transform _objectRotator;
    [SerializeField] private Vector3 _targetPosition;
    private Vector3 _targetRotation;
    [SerializeField] private Vector3 _initialSpawnOffset = Vector3.down * 5f;

    [Header("Rotation & Zoom Settings")]
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _zoomSpeed = 10f;

    private Outline _currentOutline; // Храним ссылку на Outline осматриваемого объекта

    private void Update()
    {
        if (inspectableObject == null)

            return;
        _currentOutline = inspectableObject.GetComponent<Outline>();
        if (_currentOutline != null)
            _currentOutline.enabled = false;
        ZoomInOut();
        RotateObject();
    }

    private void RotateObject()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _objectRotator.Rotate(
                new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * _rotationSpeed,
                Space.World);
        }
    }

    private void ZoomInOut()
    {
        _targetPosition = new Vector3(_targetPosition.x, _targetPosition.y,
            Mathf.Clamp(_targetPosition.z - Input.GetAxis("Mouse ScrollWheel"), inspectableObject.minMaxZoom.x,
                inspectableObject.minMaxZoom.y));

        _objectRotator.localPosition = Vector3.Lerp(_objectRotator.localPosition, _targetPosition,
            Time.deltaTime * _zoomSpeed);
    }

    private void OnEnable()
    {
        if (inspectableObject == null)
            return;

        // Отключаем Outline у осматриваемого объекта и сохраняем ссылку
        _currentOutline = inspectableObject.GetComponent<Outline>();
        if (_currentOutline != null)
            _currentOutline.enabled = false;

        _targetPosition.z = inspectableObject.defaultZoomValue;
        _objectRotator.localPosition = _initialSpawnOffset;
        _objectRotator.localRotation = Quaternion.Euler(Vector3.zero + _targetRotation);
    }

    private void OnDisable()
    {
        // При выходе из осмотра можно снова включить Outline (если нужно)
        if (_currentOutline != null)
            _currentOutline.enabled = true;

        _currentOutline = null;
        inspectableObject = null;
    }
}