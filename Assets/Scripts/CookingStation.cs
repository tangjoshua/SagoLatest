using UnityEngine;

public class CookingStation : MonoBehaviour
{
    [Header("Input")]
    public StationInput inputZone;

    [Header("Cooking Settings")]
    public float cookingTime = 5f; // ⭐ 可调时间

    [Header("Spawn")]
    public Transform outputPoint;
    public GameObject outputPrefab;
    public string outputTag = "Food";

    private GameObject currentItem;

    private bool isCooking = false;
    private float timer = 0f;

    void OnEnable()
    {
        if (inputZone != null)
            inputZone.OnItemReceived += SetItem;
    }

    void OnDisable()
    {
        if (inputZone != null)
            inputZone.OnItemReceived -= SetItem;
    }

    // ⭐ 放入物品
    void SetItem(GameObject item)
    {
        if (currentItem != null)
        {
            Debug.Log("已有物品在烹饪！");
            return;
        }

        currentItem = item;
        timer = 0f;
        isCooking = false;

        Debug.Log("物品已放入 CookingStation");
    }

    // ⭐ 由 InputManager 长按触发
    public void StartCooking()
    {
        if (currentItem == null)
        {
            Debug.Log("没有物品无法加工");
            return;
        }

        if (!isCooking)
        {
            isCooking = true;
            Debug.Log("开始烹饪！");
        }
    }

    void Update()
    {
        if (isCooking && currentItem != null)
        {
            timer += Time.deltaTime;

            Debug.Log($"Cooking: {timer}/{cookingTime}");

            if (timer >= cookingTime)
            {
                CompleteCooking();
            }
        }
    }

    void CompleteCooking()
    {
        Debug.Log("烹饪完成！");

        if (currentItem == null) return;

        // ⭐ 先拿旧数据
        ItemData oldData = currentItem.GetComponent<ItemData>();

        // ⭐ 生成新物体
        GameObject newItem = Instantiate(
            outputPrefab,
            outputPoint.position,
            outputPoint.rotation
        );

        // ⭐ 获取新数据组件
        ItemData newData = newItem.GetComponent<ItemData>();

        // ⭐ 数据继承
        if (oldData != null && newData != null)
        {
            newData.CopyFrom(oldData);
            newData.cookTime += cookingTime;
            newData.itemName = "CookedFood";
        }

        // ⭐ 再删除旧物体（顺序很重要）
        Destroy(currentItem);

        // ⭐ 确保可以拖
        if (newItem.GetComponent<DragItem>() == null)
            newItem.AddComponent<DragItem>();

        // ⭐ 设置tag
        newItem.tag = outputTag;

        // ⭐ 解锁物理（如果之前锁过）
        Rigidbody rb = newItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // ⭐ 重置状态
        currentItem = null;
        isCooking = false;
        timer = 0f;
    }


    public bool HasItem()
    {
        return currentItem != null;
    }
}
