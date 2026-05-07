using System.Collections.Generic;
using UnityEngine;

public class RefineStation : MonoBehaviour
{
    [Header("Input")]
    public StationInput inputZone;

    [Header("Spawn Points")]
    public Transform refineArea;   // ⭐ 星星生成区域
    public Transform outputPoint;  // ⭐ 成品生成点

    [Header("Refine Settings")]
    public GameObject refineTargetPrefab; // ⭐ 星星Prefab
    public int targetCount = 5;           // 生成数量

    [Header("Output")]
    public GameObject outputPrefab;
    public string outputTag = "Food";
    [Header("Refine Area Size")]
    public Vector2 areaSize = new Vector2(1.5f, 1.5f);


    private GameObject currentItem;
    private List<GameObject> activeTargets = new List<GameObject>();

    void OnEnable()
    {
        if (inputZone != null)
            inputZone.OnItemReceived += StartRefine;
    }

    void OnDisable()
    {
        if (inputZone != null)
            inputZone.OnItemReceived -= StartRefine;
    }

    // ⭐ 放入物体
    void StartRefine(GameObject item)
    {
        if (currentItem != null)
        {
            Debug.Log("正在加工中！");
            return;
        }

        currentItem = item;

        Debug.Log("开始精修");

        SpawnTargets();
    }

    // ⭐ 生成星星
    void SpawnTargets()
    {
        activeTargets.Clear();

        for (int i = 0; i < targetCount; i++)
        {
            Vector3 randomPos = refineArea.position + new Vector3(
            Random.Range(-areaSize.x / 2, areaSize.x / 2),
            0.2f,
            Random.Range(-areaSize.y / 2, areaSize.y / 2)
            );


            GameObject target = Instantiate(refineTargetPrefab, randomPos, Quaternion.identity);

            // ⭐ 绑定回调
            RefineTarget rt = target.GetComponent<RefineTarget>();
            if (rt != null)
            {
                rt.SetStation(this);
            }

            activeTargets.Add(target);
        }
    }

    // ⭐ 每销毁一个调用
    public void OnTargetDestroyed(GameObject target)
    {
        activeTargets.Remove(target);

        if (activeTargets.Count == 0)
        {
            CompleteRefine();
        }
    }

    // ⭐ 完成
    void CompleteRefine()
    {
        Debug.Log("精修完成！");

        if (currentItem == null) return;

        // ⭐ 1. 先拿旧数据
        ItemData oldData = currentItem.GetComponent<ItemData>();

        // ⭐ 2. 生成新物体
        GameObject newItem = Instantiate(
            outputPrefab,
            outputPoint.position,
            outputPoint.rotation
        );

        // ⭐ 3. 获取新数据
        ItemData newData = newItem.GetComponent<ItemData>();

        // ⭐ 4. 数据继承
        if (oldData != null && newData != null)
        {
            newData.CopyFrom(oldData);
            newData.refineCount += 1;
        }

        // ⭐ 5. 再删除旧物体
        Destroy(currentItem);

        // ⭐ 6. 确保可拖拽
        if (newItem.GetComponent<DragItem>() == null)
            newItem.AddComponent<DragItem>();

        // ⭐ 7. 设置 tag
        newItem.tag = outputTag;

        // ⭐ 8. 解锁物理（如果之前锁过）
        Rigidbody rb = newItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // ⭐ 9. 清空状态
        currentItem = null;
    }

}
