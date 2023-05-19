using UnityEngine;
using System.Collections;

public class CameraPinchToZoom : MonoBehaviour
{
    [Header("Camera Limits if is Orthographic")]
    [SerializeField]
    private float _ortographicLowLimit;

    [SerializeField]
    private float _ortographicHighLimit;

    [Header("Camera Limits if is NON - Orthographic")]

    [SerializeField]
    private float _nonOrtographicLowLimit;

    [SerializeField]
    private float _nonOrtographicHighLimit;

    public float perspectiveZoomSpeed = 0.5f;
    public float orthoZoomSpeed = 0.5f;

    private float _prevTouchDeltaMag;
    private float _touchDeltaMag;
    private float _deltaMagnitudeDiff;

    private float _clampLowLimit;
    private float _clampHighLimit;
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            _prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            _touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            _deltaMagnitudeDiff = _prevTouchDeltaMag - _touchDeltaMag;

            if (GetComponent<Camera>().orthographic)
            {
                _clampHighLimit = _ortographicHighLimit;
                _clampLowLimit = _ortographicLowLimit;
                GetComponent<Camera>().orthographicSize += _deltaMagnitudeDiff * orthoZoomSpeed;
                GetComponent<Camera>().orthographicSize = Mathf.Clamp(GetComponent<Camera>().orthographicSize, _clampLowLimit, _clampHighLimit);
            }
            else
            {
                _clampHighLimit = _nonOrtographicHighLimit;
                _clampLowLimit= _nonOrtographicLowLimit;
                GetComponent<Camera>().fieldOfView += _deltaMagnitudeDiff * perspectiveZoomSpeed;
                GetComponent<Camera>().fieldOfView = Mathf.Clamp(GetComponent<Camera>().fieldOfView, _clampLowLimit, _clampHighLimit);
            }
        }
    }
}