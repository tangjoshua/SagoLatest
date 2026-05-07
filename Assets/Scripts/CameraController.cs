using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Points")]
    public List<Transform> cameraPoints;

    [Header("Settings")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 5f;

    private Transform targetPoint;
    private bool isMoving = false;

    void Update()
    {
        if (targetPoint == null) return;

        // 平滑移动
        transform.position = Vector3.Lerp(
            transform.position,
            targetPoint.position,
            Time.deltaTime * moveSpeed
        );

        // 平滑旋转
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetPoint.rotation,
            Time.deltaTime * rotateSpeed
        );

        // 到达检测（防止一直插值）
        float dist = Vector3.Distance(transform.position, targetPoint.position);

        if (dist < 0.01f)
        {
            isMoving = false;
        }
    }

    public void GoToPoint(int index)
    {
        // ❗ 输入中禁止切换（防冲突）
        if (InputManager.IsUsingInput)
        {
            Debug.Log("Blocked camera switch (input in use)");
            return;
        }

        if (index < 0 || index >= cameraPoints.Count)
        {
            Debug.LogWarning("Invalid camera index");
            return;
        }

        targetPoint = cameraPoints[index];
        isMoving = true;

        Debug.Log("Switching to: " + targetPoint.name);
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
