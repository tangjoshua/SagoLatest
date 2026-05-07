using UnityEngine;

public class ServeStation : MonoBehaviour
{
    [Header("Input Zones")]
    public StationInput orderZone;
    public StationInput foodZone;

    [Header("NPC Spawn")]
    public Transform npcSpawnPoint;

    private OrderPaper currentOrder;
    private NPCOrder currentNPC;
    private GameObject currentFood;

    private bool isProcessing = false;

    void OnEnable()
    {
        orderZone.OnItemReceived += OnOrderPlaced;
        foodZone.OnItemReceived += OnFoodPlaced;
    }

    void OnDisable()
    {
        orderZone.OnItemReceived -= OnOrderPlaced;
        foodZone.OnItemReceived -= OnFoodPlaced;
    }


    // =========================
    // ⭐ 订单入口
    // =========================
    void OnOrderPlaced(GameObject item)
    {
        Debug.Log("👉 OrderZone触发");

        if (!item.CompareTag("Order"))
        {
            Debug.Log("❌ 不是订单");
            return;
        }

        if (currentOrder != null)
        {
            Debug.Log("已有订单");
            return;
        }

        OrderPaper order = item.GetComponent<OrderPaper>();
        if (order == null)
        {
            Debug.Log("❌ 没有 OrderPaper 脚本");
            return;
        }

        currentOrder = order;

        LockItem(item);

        Debug.Log("📄 订单已放置");

        SpawnNPC(order);
    }

    // =========================
    // ⭐ 食物入口
    // =========================
    void OnFoodPlaced(GameObject item)
    {
        Debug.Log("👉 FoodZone触发");

        if (!item.CompareTag("Sago"))
        {
            Debug.Log("❌ 不是食物");
            return;
        }

        if (currentOrder == null || currentNPC == null)
        {
            Debug.Log("⚠ 没有订单或NPC");
            return;
        }

        if (currentFood != null)
        {
            Debug.Log("已有食物");
            return;
        }

        currentFood = item;

        LockItem(item);

        CheckFood();
    }

    // =========================
    void SpawnNPC(OrderPaper order)
    {
        if (order.npcPrefab == null || npcSpawnPoint == null)
        {
            Debug.LogWarning("NPC prefab 或 spawn point 未设置");
            return;
        }

        GameObject npcObj = Instantiate(
            order.npcPrefab,
            npcSpawnPoint.position,
            npcSpawnPoint.rotation
        );

        currentNPC = npcObj.GetComponent<NPCOrder>();

        if (currentNPC != null && currentOrder != null)
        {
            currentNPC.InitAsServeNPC(currentOrder.orderData);
        }
        Debug.Log("👤 NPC已生成");
    }

    // =========================
    void CheckFood()
    {
        isProcessing = true;

        bool success = currentOrder.CheckFood(currentFood);

        if (success)
        {
            Debug.Log("✅ 完成订单");

            if (currentNPC != null)
            {
                currentNPC.OnOrderCompleted();
            }

            FinishOrder();
        }
        else
        {
            Debug.Log("❌ 食物不符合要求");
            isProcessing = false;
        }
    }

    void FinishOrder()
    {
        if (currentFood != null)
            Destroy(currentFood);

        if (currentOrder != null)
        {
            if (currentOrder.currentSlot != null)
            {
                currentOrder.currentSlot.ClearSlot();
            }

            Destroy(currentOrder.gameObject);
        }

        if (currentNPC != null)
            Destroy(currentNPC.gameObject);

        ClearAll();
    }
    void LockItem(GameObject obj)
    {
        DragItem drag = obj.GetComponent<DragItem>();
        if (drag != null)
            drag.StopDrag();

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;
    }

    void ClearAll()
    {
        currentOrder = null;
        currentNPC = null;
        currentFood = null;
        isProcessing = false;
    }
}
