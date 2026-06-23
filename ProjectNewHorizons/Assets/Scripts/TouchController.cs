using UnityEngine;

public class TouchController
{
    private Vector3 _startTouchInWorldSpace;
    public Vector3 GetWorldPosition(Vector2 screenPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit))
        {
            return hit.point;
        }

        Debug.LogWarning("Raycast missed!");
        return _startTouchInWorldSpace;
    }
}
