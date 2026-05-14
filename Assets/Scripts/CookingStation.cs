using UnityEngine;

public class CookingStation : MonoBehaviour
{
    [Header("Input")]
    public StationInput inputZone;

    [Header("Cooking Area")]
    // ⭐ 新增：拖入一个带 Trigger 属性的 Collider（比如一个透明的方块）
    public Collider cookingArea; 

    [Header("Cooking Settings")]
    public float cookingTime = 5f;

    [Header("Spawn")]
    public Transform outputPoint;
    public GameObject outputPrefab;
    public string outputTag = "Food";

    private GameObject currentItem;
    private bool isCooking = false;
    private float timer = 0f;

    void OnEnable() { if (inputZone != null) inputZone.OnItemReceived += SetItem; }
    void OnDisable() { if (inputZone != null) inputZone.OnItemReceived -= SetItem; }

    void SetItem(GameObject item)
    {
        if (currentItem != null) return;
        currentItem = item;
        timer = 0f;
        isCooking = false;
        if (ProgressUIManager.Instance != null) ProgressUIManager.Instance.Hide();
    }

    // ⭐ 核心逻辑修改：刷子调用时判断区域
    public void BrushCook(Vector3 brushPosition)
    {
        if (currentItem == null) return;

        // 检查刷子是否在规定的加工区域内
        if (cookingArea != null && !cookingArea.bounds.Contains(brushPosition))
        {
            // 如果不在区域内，可以隐藏进度条或直接返回
            // ProgressUIManager.Instance.Hide(); 
            return;
        }

        isCooking = true;
        timer += Time.deltaTime;

        if (ProgressUIManager.Instance != null)
        {
            ProgressUIManager.Instance.UpdateProgress(timer, cookingTime);
        }

        if (timer >= cookingTime)
        {
            CompleteCooking();
        }
    }

    void CompleteCooking()
    {
        if (currentItem == null) return;
        if (ProgressUIManager.Instance != null) ProgressUIManager.Instance.Hide();

        ItemData oldData = currentItem.GetComponent<ItemData>();
        GameObject newItem = Instantiate(outputPrefab, outputPoint.position, outputPoint.rotation);
        ItemData newData = newItem.GetComponent<ItemData>();

        if (oldData != null && newData != null)
        {
            newData.CopyFrom(oldData);
            newData.cookTime += cookingTime;
            newData.itemName = "CookedFood";
        }

        Destroy(currentItem);
        if (newItem.GetComponent<DragItem>() == null) newItem.AddComponent<DragItem>();
        newItem.tag = outputTag;

        Rigidbody rb = newItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        currentItem = null;
        isCooking = false;
        timer = 0f;
    }
}