using System.Collections.Generic;
using UnityEngine;

public class StationInput : MonoBehaviour
{
    [Header("Accepted Tags")]
    public List<string> acceptTags = new List<string>();

    public Transform snapPoint;

    public System.Action<GameObject> OnItemReceived;

    private void OnTriggerEnter(Collider other)
    {
        // ⭐ 获取“真实物体”（关键修复点）
        GameObject root = other.attachedRigidbody
            ? other.attachedRigidbody.gameObject
            : other.transform.root.gameObject;

        Debug.Log($"进入 {gameObject.name} | 实际物体: {root.name} | Tag: {root.tag}");

        // ⭐ 正确Tag判断（用root）
        if (!acceptTags.Contains(root.tag))
        {
            Debug.Log($"❌ {gameObject.name} 拒绝 {root.tag}");
            return;
        }

        Debug.Log($"✅ {gameObject.name} 接受 {root.tag}");

        // ⭐ 用root拿DragItem（不是other）
        DragItem drag = root.GetComponent<DragItem>();

        if (drag == null)
        {
            Debug.Log("❌ 没有 DragItem");
            return;
        }

        if (!drag.IsDragging())
        {
            Debug.Log("❌ 不是拖拽状态");
            return;
        }

        ReceiveItem(root);
    }

    public void ReceiveItem(GameObject item)
    {
        // ⭐ Snap
        if (snapPoint != null)
        {
            item.transform.position = snapPoint.position;
            item.transform.rotation = snapPoint.rotation;
        }

        // ⭐ 停止拖拽
        DragItem drag = item.GetComponent<DragItem>();
        if (drag != null)
        {
            drag.StopDrag();
        }

        // ⭐ 锁物理
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("🎯 成功进入输入区: " + item.name);

        OnItemReceived?.Invoke(item);
    }
}
