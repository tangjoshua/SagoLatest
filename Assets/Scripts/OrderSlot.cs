using UnityEngine;

public class OrderSlot : MonoBehaviour
{
    public OrderPaper currentOrder;

    // =========================
    public bool IsEmpty()
    {
        return currentOrder == null;
    }

    // =========================
    public void SetOrder(OrderPaper order)
    {
        currentOrder = order;

        order.currentSlot = this;

        order.transform.SetParent(transform);

        order.transform.localPosition = Vector3.zero;
        order.transform.localRotation = Quaternion.identity;

        Debug.Log("订单进入Slot");
    }

    // =========================
    public void ClearSlot()
    {
        currentOrder = null;

        Debug.Log("Slot已清空");
    }
}