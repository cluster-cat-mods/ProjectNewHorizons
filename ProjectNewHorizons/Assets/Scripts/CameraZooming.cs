using System;
using System.Collections;
using UnityEngine;

public class CameraZooming : MonoBehaviour
{
    [SerializeField] private float zoomScalar = 175;
    
    private bool _zooming = false;
    private float zoomLevel;
    private void Update()
    {
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
            distance /= 200;
            zoomLevel += distance;
            zoomLevel = Mathf.Clamp(zoomLevel, 0f, 1f);
            
            Vector3 newPos = transform.forward * zoomLevel * zoomScalar;
            transform.localPosition = newPos;
            startDist = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            yield return null;
        }
        _zooming = false;
        yield return null;
    }
}
