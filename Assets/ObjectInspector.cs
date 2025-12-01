using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class ObjectInspector : MonoBehaviour
{
    private GameObject _inspectableObject;
    [SerializeField] private CinemachineVirtualCamera _cvc;
    
    [Header("Button Setup")]
    [SerializeField] private KeyCode _openInspectionButton = KeyCode.F;
    [SerializeField] private KeyCode _closeInspectionButton = KeyCode.Mouse1;

    [Header("Pick up Settings")]
    [SerializeField] private Camera _maincamera;
    [SerializeField] private float _reachDistance = 10f;

    [SerializeField] private InspectionCamera _inspectionCamera;

    [SerializeField] private RaycastOutline _raycastOutline;

    [SerializeField] private GameObject _inspectionCanvas;
    //[SerializeField] private GameObject _mainCanvas;

    void Update()
    {
        if (_inspectableObject == null)
        {
            Ray ray = _maincamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Input.GetKeyDown(_openInspectionButton))
            {
                if (Physics.Raycast(ray, out hit, _reachDistance))
                {
                    if (hit.collider.gameObject.GetComponent<InspectableObject>() != null)
                    {
                        _inspectableObject = Instantiate(hit.collider.gameObject, _inspectionCamera.transform.GetChild(0));
                        _inspectableObject.GetComponent<Rigidbody>().isKinematic = true;
                        InspectableObject inspectableObject = _inspectableObject.GetComponent<InspectableObject>();
                        
                        inspectableObject.transform.localPosition =
                            Vector3.zero + inspectableObject.spawnPositionOffset;
                        inspectableObject.transform.localRotation =
                            Quaternion.Euler(Vector3.zero + inspectableObject.spawnRotationOffset);

                        _inspectionCanvas.SetActive(true);

                        _inspectionCamera.inspectableObject = inspectableObject;
                        _inspectionCamera.gameObject.SetActive(true);

                        //_mainCanvas.SetActive(false);
                        TurnOffCameraMovement();
                    }
                }
            }
        }

        if (Input.GetKeyDown(_closeInspectionButton))
        {
            Destroy(_inspectableObject);
            _inspectionCanvas.SetActive(false);
            _inspectionCamera.gameObject.SetActive(false);
            //_mainCanvas.SetActive(true);
            TurnOnCameraMovement();
        }
    }

    private void TurnOnCameraMovement()
    {
        CinemachinePOV pov = _cvc.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.m_InputAxisName = "Mouse X";
        pov.m_VerticalAxis.m_InputAxisName = "Mouse Y";

        if (_raycastOutline != null)
        {
            _raycastOutline.enabled = true;
        }


    }
    private void TurnOffCameraMovement()
    {
        CinemachinePOV pov = _cvc.GetCinemachineComponent<CinemachinePOV>();
        pov.m_HorizontalAxis.m_InputAxisName = "";
        pov.m_VerticalAxis.m_InputAxisName = "";
        pov.m_HorizontalAxis.m_InputAxisValue = 0;
        pov.m_VerticalAxis.m_InputAxisValue = 0;

        // Отключаем RaycastOutline при осмотре
        if (_raycastOutline != null)
        {
            _raycastOutline.enabled = false;
        }
    }
}
