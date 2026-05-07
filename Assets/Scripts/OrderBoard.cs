using UnityEngine;

public class OrderBoard : MonoBehaviour
{
    public static OrderBoard Instance;

    public OrderSlot[] slots;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    public bool AddOrder(OrderPaper order)
    {
        foreach (OrderSlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetOrder(order);

                return true;
            }
        }

        Debug.Log("❌ 没有空Slot");

        return false;
    }
}