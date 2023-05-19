using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;
    private float _dragSpeed = 0.01f;
    private float _timeDragStarted;
    private Vector3 _previousPosition = Vector3.zero;
    private float _xLimit = 13.36336f;
    private float _yLimit = 2.715f;
    private float _timeDragLimit = 0.05f;
    private float _dragSpeedConstant = 0.002f;

    private float _deltaX;
    private float _deltaY;
    private float _newX;
    private float _newY;

    public SlingShot SlingShot;

    void Update()
    {
        if (SlingShot.slingshotState == SlingshotState.Idle && GameManager.CurrentGameState == GameState.Playing)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _timeDragStarted = Time.time;
                _dragSpeed = 0f;
                _previousPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && Time.time - _timeDragStarted > _timeDragLimit)
            {
                Vector3 input = Input.mousePosition;
                 _deltaX = (_previousPosition.x - input.x)  * _dragSpeed;
                 _deltaY = (_previousPosition.y - input.y) * _dragSpeed;
                 _newX = Mathf.Clamp(transform.position.x + _deltaX, _leftLimit.transform.position.x, _rightLimit.transform.position.x);
                 _newY = Mathf.Clamp(transform.position.y + _deltaY, 0, _yLimit);
                transform.position = new Vector3(
                    _newX,
                    _newY,
                    transform.position.z);

                _previousPosition = input;
                if(_dragSpeed < 0.1f) _dragSpeed += _dragSpeedConstant;
            }
        }
    }
}
