using UnityEngine;

public class BrushDrag : MonoBehaviour
{
    public CookingStation station;

    private Vector3 startPos;

    private bool isDragging = false;

    void Start()
    {
        startPos = transform.position;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;
        Debug.Log("Dragging Brush");

        DragBrush();

        if (station != null)
        {
            station.BrushCook(transform.position);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;

        ReturnToStart();
    }

    void DragBrush()
    {
        Ray ray =
            Camera.main.ScreenPointToRay(Input.mousePosition);

        Plane plane =
            new Plane(Vector3.up, Vector3.zero);

        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 point =
                ray.GetPoint(distance);

            transform.position =
                new Vector3(
                    point.x,
                    transform.position.y,
                    point.z
                );
        }
    }

    void ReturnToStart()
    {
        transform.position = startPos;
    }
}