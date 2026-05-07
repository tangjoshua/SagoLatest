using UnityEngine;

public class StationOutput : MonoBehaviour
{
    public StationInput nextStation; // ⭐ 下一站

    private void OnTriggerEnter(Collider other)
    {
        DragItem drag = other.GetComponent<DragItem>();

        if (drag == null) return;

        Debug.Log("Item entered Output: " + other.name);

        SendToNext(other.gameObject);
    }

    void SendToNext(GameObject item)
    {
        if (nextStation == null)
        {
            Debug.LogWarning("No next station assigned!");
            return;
        }

        // ⭐ 停止拖拽（确保不是玩家控制）
        DragItem drag = item.GetComponent<DragItem>();
        if (drag != null)
        {
            drag.StopDrag();
        }

        // ⭐ 发送到下一个站
        nextStation.SendMessage("ReceiveItem", item);
    }
}
