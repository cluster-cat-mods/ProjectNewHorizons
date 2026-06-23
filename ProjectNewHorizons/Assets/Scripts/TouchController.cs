using UnityEngine;

public class TouchController : MonoBehaviour
{
    public Touch touch;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            touch = Input.GetTouch(0);
        }
    }
    public Vector3 GetWorldPosition()
    {
        RaycastHit hit;
        hit = GetHit();
        return hit.point;
    }

    public RaycastHit GetHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
        {
            return hit;
        }
        Debug.LogWarning("Raycast missed!");
        return hit;
    }
}
