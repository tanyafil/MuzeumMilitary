using UnityEngine;

public class RaycastOutline : MonoBehaviour
{
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private InspectionCamera _inspectionCamera;
    private float _maxRayDistance = 10f;
    private Outline _lastOutlineObject;

    void Update()
    {
        // Если сейчас осматриваем предмет, не включаем подсветку
        if (_inspectionCamera != null && _inspectionCamera.inspectableObject != null)
        {
            if (_lastOutlineObject != null)
            {
                _lastOutlineObject.enabled = false;
                _lastOutlineObject = null;
            }
            return;
        }

        Debug.DrawRay(_playerCamera.transform.position, _playerCamera.transform.forward * _maxRayDistance, Color.green);

        // Отключаем предыдущий outline, если он есть
        if (_lastOutlineObject != null)
        {
            _lastOutlineObject.enabled = false;
            _lastOutlineObject = null;
        }

        RaycastHit hit;
        if (Physics.Raycast(_playerCamera.transform.position, _playerCamera.transform.forward, out hit, _maxRayDistance))
        {
            if (hit.transform.CompareTag("Item"))
            {
                Outline outline = hit.transform.GetComponent<Outline>();
                if (outline != null)
                {
                    outline.enabled = true;
                    _lastOutlineObject = outline;
                }
            }
        }
    }
}