using UnityEngine;

public class ShapeStation : MonoBehaviour
{
    [Header("Settings")]
    public float requiredProgress = 5f;
    public float progressPerSwipe = 0.5f;
    public float swipeThreshold = 50f;

    [Header("Dough Stickiness")]
    public float initialStickiness = 10f;
    public float stickinessDecreasePerFlour = 2f;
    public float requiredStickinessForCompletion = 2f;

    [Header("Prefab")]
    public GameObject outputPrefab;

    [Header("Station")]
    public StationInput doughInputZone;
    public StationInput flourInputZone;
    public Transform spawnPoint;

    private float currentProgress = 0f;
    private float currentStickiness;
    private GameObject currentDough;

    private Vector2 touchStartPos;
    private bool isSwiping = false;

   void OnEnable()
    {
        if (doughInputZone != null)
            doughInputZone.OnItemReceived += SetDough;

        if (flourInputZone != null)
            flourInputZone.OnItemReceived += AddFlour;
    }

    void OnDisable()
    {
        if (doughInputZone != null)
            doughInputZone.OnItemReceived -= SetDough;

        if (flourInputZone != null)
            flourInputZone.OnItemReceived -= AddFlour;
    }


    void Start()
    {
        currentStickiness = initialStickiness;
    }

    // ⭐ 统一入口（核心）


    public void SetDough(GameObject dough)
    {
        if (currentDough != null)
        {
            Debug.LogWarning("已有面团！");
            return;
        }

        currentDough = dough;
        currentProgress = 0f;
        currentStickiness = initialStickiness;

        Debug.Log("面团已就位");
    }

    public void ProcessInput()
    {
        if (currentDough == null)
        {
            Debug.Log("工作台是空的，请放入面团。");
        }
        else
        {
            Debug.Log($"进度: {currentProgress}, 粘稠度: {currentStickiness}");
        }
    }

    void Update()
    {
        if (currentDough != null)
        {
            HandleSwipeInput();
        }
    }

    void HandleSwipeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPos = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isSwiping = false;
        }

        if (isSwiping && Input.GetMouseButton(0))
        {
            Vector2 currentPos = Input.mousePosition;
            float distance = Vector2.Distance(touchStartPos, currentPos);

            if (distance > swipeThreshold)
            {
                currentProgress += progressPerSwipe;
                Debug.Log($"加工中: {currentProgress}/{requiredProgress}");

                touchStartPos = currentPos;
            }

            if (currentProgress >= requiredProgress)
            {
                if (currentStickiness <= requiredStickinessForCompletion)
                {
                    CompleteShape();
                }
                else
                {
                    Debug.Log("太粘了！需要加面粉");
                }
            }
        }
    }

    public void AddFlour(GameObject flour)
    {
        if (currentDough == null)
        {
            Debug.Log("没面团，加面粉无效");
            Destroy(flour);
            return;
        }

        currentStickiness = Mathf.Max(0f, currentStickiness - stickinessDecreasePerFlour);

        Debug.Log("粘稠度下降: " + currentStickiness);

        Destroy(flour);
    }


    void CompleteShape()
    {
        GameObject newItem = Instantiate(
            outputPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // ⭐ 继承数据
        ItemData oldData = currentDough.GetComponent<ItemData>();
        ItemData newData = newItem.GetComponent<ItemData>();

        if (oldData != null && newData != null)
        {
            newData.CopyFrom(oldData);
            newData.shapeProgress += 1;
        }

        Destroy(currentDough);

        currentDough = null;
    }

    public bool HasDough()
    {
        return currentDough != null;
    }
}
