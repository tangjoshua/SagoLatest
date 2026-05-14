using UnityEngine;

public class RefineStation : MonoBehaviour
{
    [Header("Input")]
    public StationInput inputZone;

    [Header("Spawn Points")]
    public Transform outputPoint;  

    [Header("Refine Settings")]
    public float refineTime = 3f;         

    [Header("Output")]
    public GameObject outputPrefab;
    public string outputTag = "Food";

    private GameObject currentItem;
    private bool isRefining = false;      
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

    void SetItem(GameObject item)
    {
        if (currentItem != null) return;

        currentItem = item;
        timer = 0f;
        isRefining = false;

        if (ProgressUIManager.Instance != null) ProgressUIManager.Instance.Hide();
        Debug.Log($"<color=cyan>[RefineStation]</color> 物品 {item.name} 已放入，等待加工。");
    }

    // ⭐ 重要：长按时，InputManager 每一帧都应该调用这个函数
    public void StartRefining()
    {
        if (currentItem == null)
        {
            Debug.LogWarning("<color=yellow>[RefineStation]</color> 尝试加工，但台上没有物品！");
            return;
        }
        
        if (!isRefining)
        {
            Debug.Log("<color=green>[RefineStation]</color> 检测到长按：加工开始");
        }
        
        isRefining = true;
    }

    // ⭐ 重要：松开按键时，必须调用这个函数重置状态
    public void StopRefining()
    {
        if (isRefining)
        {
            Debug.Log("<color=orange>[RefineStation]</color> 长按中断：加工停止");
        }
        isRefining = false;
    }

    void Update()
    {
        if (isRefining && currentItem != null)
        {
            timer += Time.deltaTime;

            // 实时打印进度日志
            Debug.Log($"<color=white>[RefineStation]</color> 加工中进度: {timer:F2} / {refineTime:F2}");

            if (ProgressUIManager.Instance != null)
            {
                ProgressUIManager.Instance.UpdateProgress(timer, refineTime);
            }
            else
            {
                Debug.LogError("<color=red>[RefineStation]</color> 找不到 ProgressUIManager 实例！请确保场景中有此脚本。");
            }

            if (timer >= refineTime)
            {
                CompleteRefine();
            }
        }
        
        // 关键：为了防止状态锁死，如果一帧内没有调用 StartRefining，应该在逻辑上处理
        // 这里我们依靠 InputManager 的每帧调用来维持 isRefining。
        // 所以在 Update 的最后重置状态，确保只有按住时才为 true。
        isRefining = false; 
    }

    void CompleteRefine()
    {
        Debug.Log("<color=gold>[RefineStation]</color> 加工完成！正在生成成品...");

        if (currentItem == null) return;

        if (ProgressUIManager.Instance != null) ProgressUIManager.Instance.Hide();

        ItemData oldData = currentItem.GetComponent<ItemData>();
        GameObject newItem = Instantiate(outputPrefab, outputPoint.position, outputPoint.rotation);
        ItemData newData = newItem.GetComponent<ItemData>();

        if (oldData != null && newData != null)
        {
            newData.CopyFrom(oldData);
            newData.refineCount += 1;
        }

        Destroy(currentItem);

        if (newItem.GetComponent<DragItem>() == null)
            newItem.AddComponent<DragItem>();

        newItem.tag = outputTag;

        Rigidbody rb = newItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        currentItem = null;
        timer = 0f;
        isRefining = false;
    }
}