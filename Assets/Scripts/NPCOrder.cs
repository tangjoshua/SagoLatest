using UnityEngine;

public class NPCOrder : MonoBehaviour
{
    [Header("Order Data")]
    public OrderData orderData;

    [Header("Order Paper")]
    public GameObject orderPaperPrefab;
    public Transform spawnPoint;

    private bool hasOrdered = false;
    private bool isServeNPC = false; // ⭐ 新增

    // ⭐ 关键：Spawner引用
    private NPCSpawner spawner;

    // ⭐ 给Spawner绑定
    public void SetSpawner(NPCSpawner s)
    {
        spawner = s;
    }
    
    public void InitAsServeNPC(OrderData data)
    {
        orderData = data;
        isServeNPC = true;
    }

    // ⭐ 点击NPC → 生成订单纸
    public void OnClickOrder()
    {
        // ❌ ServeStation生成的NPC不能点
        if (isServeNPC)
        {
            Debug.Log("这是服务NPC，不生成订单");
            return;
        }

        if (hasOrdered) return;

        hasOrdered = true;

        if (orderPaperPrefab == null || spawnPoint == null)
        {
            Debug.LogError("OrderPaperPrefab 或 SpawnPoint 未设置！");
            return;
        }

        GameObject paper = Instantiate(
            orderPaperPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        paper.tag = "Order";

        OrderPaper op = paper.GetComponent<OrderPaper>();

        if (op != null)
        {
            op.SetOrder(orderData);
        }

        // ⭐ 关键：生成订单后NPC消失
        Destroy(gameObject);
    }

    // ⭐ ServeStation 会调用这个（核心）
    public bool ReceiveFood(GameObject food)
    {
        if (orderData == null)
        {
            Debug.LogWarning("没有订单数据");
            return false;
        }

        // ⭐ 基础判定（你可以扩展）
        if (!food.CompareTag(orderData.requiredTag))
        {
            Debug.Log("❌ 食物Tag不符合");
            return false;
        }

        Debug.Log("✅ 食物符合要求");

        // ⭐ 完成订单
        OnOrderCompleted();

        return true;
    }

    // ⭐ 完成订单 → 通知Spawner
    public void OnOrderCompleted()
    {
        Debug.Log("🎉 NPC订单完成");

        // ⭐ NPC完成后的文本
        DialogueUI.Instance.StartDialogue(
            new System.Collections.Generic.List<string>()
            {
                "Thank you for the food!",
                "It's delicious!"
            }
        );

        // ⭐ 通知DayManager
        DayManager.Instance.SpawnNextNPC();

        Destroy(gameObject, 1f);
    }

}
