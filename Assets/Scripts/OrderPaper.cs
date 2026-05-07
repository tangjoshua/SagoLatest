using UnityEngine;

public class OrderPaper : MonoBehaviour
{
    public OrderData orderData;

    [Header("NPC")]
    public GameObject npcPrefab;

    private bool isInBoard = false;

    [HideInInspector]
    public OrderSlot currentSlot;

    // =========================
    // ⭐ NPC生成订单时调用
    // =========================
    public void SetOrder(OrderData data)
    {
        orderData = data;
    }

    // =========================
    // ⭐ 点击订单
    // =========================
    void OnMouseDown()
    {
        // 第一次点击 → 收进订单板
        if (!isInBoard)
        {
            if (OrderBoard.Instance != null)
            {
                bool success = OrderBoard.Instance.AddOrder(this);

                if (success)
                {
                    isInBoard = true;
                }
            }

            return;
        }

        // 第二次点击 → 打开详情
        if (OrderUI.Instance != null)
        {
            OrderUI.Instance.ShowOrder(orderData);
        }
    }

    // =========================
    public void SetInBoard()
    {
        isInBoard = true;
    }

    public bool CheckFood(GameObject food)
    {
        ItemData data = food.GetComponent<ItemData>();

        if (data == null)
        {
            Debug.Log("❌ Food没有ItemData");
            return false;
        }

        // ⭐ Tag检查
        if (data.itemTag != orderData.requiredTag)
        {
            Debug.Log("❌ Tag不符合");
            return false;
        }

        // ⭐ 材料检查
        foreach (var req in orderData.requiredIngredients)
        {
            if (!data.ingredients.Contains(req))
            {
                Debug.Log("❌ 缺少材料: " + req);
                return false;
            }
        }

        // ⭐ CookTime检查
        if (data.cookTime < orderData.minCookTime)
        {
            Debug.Log("❌ CookTime不足");
            return false;
        }

        // ⭐ Refine检查
        if (data.refineCount < orderData.minRefine)
        {
            Debug.Log("❌ Refine不足");
            return false;
        }

        Debug.Log("✅ 食物符合订单");

        return true;
    }
}