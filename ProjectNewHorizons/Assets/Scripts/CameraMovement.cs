using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private TouchController touchController;

    [SerializeField] private Vector2 movementScalar = new(16, 8);
    [SerializeField] private float zoomScalar = 175;
    [SerializeField] private Vector2 borderClamp = new(2000,1080);
    private Vector3 _cameraPosition;
    private Vector3 _startTouchInWorldSpace;
    private bool _zooming = false;
    
    private void Start()
    {
        if (touchController == null)
        {
            touchController = FindAnyObjectByType<TouchController>();
        }

        _cameraPosition = transform.position;
    }
    private void Update()
    {
        if (Input.touchCount == 1)
        {
            if (touchController.touch.phase == TouchPhase.Began)
            {
                _startTouchInWorldSpace = touchController.GetWorldPosition();
            }

            if (touchController.touch.phase == TouchPhase.Moved)
            {
                var _currentWorldPosition = touchController.GetWorldPosition();
                var _worldDifference = _currentWorldPosition - _startTouchInWorldSpace;

                _cameraPosition = transform.position;

                var _newCameraPosition = _cameraPosition + new Vector3(-_worldDifference.x, 0, -_worldDifference.z);

                _newCameraPosition.x = Mathf.Clamp(
                    _newCameraPosition.x,
                    -borderClamp.x / movementScalar.x / 2f,
                    borderClamp.x / movementScalar.x / 2f
                );

                _newCameraPosition.z = Mathf.Clamp(
                    _newCameraPosition.z,
                    -borderClamp.y / movementScalar.y / 2f - 20,
                    borderClamp.y / movementScalar.y / 2f - 20
                );

                transform.position = _newCameraPosition;
            }


            //logging
            //Debug.Log($"finger {touch.fingerId} tapped {touch.tapCount} times");
            //Debug.Log($"time since last touch value change = {touch.deltaTime}");
            //Debug.Log($"position since last change in pixel cords = {touch.deltaPosition}");
            //Debug.Log($"the phase of the touch = {touch.phase}");
        }
        
        if (Input.touchCount == 2 && !_zooming)
        {
            Debug.Log("Start zooming");
            StartCoroutine(ZoomCoroutine());
            _zooming = true;
        }

    }

    private IEnumerator ZoomCoroutine()
    {
        float startDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        while (Input.touchCount == 2)
        {
            float distance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) - startDist;
            
            Vector3 newPos = transform.localPosition + transform.forward * distance / zoomScalar;
            newPos.y = Mathf.Clamp(newPos.y, 5, 25);
            newPos.z = Mathf.Clamp(newPos.x, -35, -25);
            startDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            yield return null;
        }
        _zooming = false;
        yield return null;
    }
        
}
