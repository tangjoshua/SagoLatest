using UnityEngine;

public class DragItem : MonoBehaviour
{
    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    public void StartDrag()
    {
        isDragging = true;
        transform.position += Vector3.up * 0.1f; // ⭐ 稍微抬高，避免穿透
    }

    public void StopDrag()
    {
        isDragging = false;
    }

    void Update()
    {
        if (!isDragging) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform) return;

            transform.position = hit.point + Vector3.up * 0.05f;
        }
    }



    public bool IsDragging()
    {
        return isDragging;
    }
}
