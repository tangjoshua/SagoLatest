using UnityEngine;
using UnityEngine.UI; // 必须引用
using TMPro;           // 必须引用

public class ShapeStation : MonoBehaviour
{
    [Header("UI References")]
    public Slider interactionSlider;      // ⭐ 你在 shapeUI 里的那个左右滑动条
    public TextMeshProUGUI warningText;   // ⭐ 提示“加入面粉”的文字

    [Header("Settings")]
    public float requiredProgress = 5f;
    public float progressPerMove = 0.2f;  // 每次滑动条变动增加的进度

    [Header("Dough Stickiness")]
    public float initialStickiness = 10f;
    public float stickinessDecreasePerFlour = 2f;
    public float requiredStickinessForCompletion = 2f;
    public float stickinessThreshold = 8f; // ⭐ 粘稠度超过这个值就停止加工

    [Header("Prefab")]
    public GameObject outputPrefab;

    [Header("Station")]
    public StationInput doughInputZone;
    public StationInput flourInputZone;
    public Transform spawnPoint;

    private float currentProgress = 0f;
    private float currentStickiness;
    private GameObject currentDough;
    private float lastSliderValue; // 用于记录滑动条上一次的位置

    void OnEnable()
    {
        if (doughInputZone != null)
            doughInputZone.OnItemReceived += SetDough;

        if (flourInputZone != null)
            flourInputZone.OnItemReceived += AddFlour;

        // 绑定 Slider 事件
        if (interactionSlider != null)
            interactionSlider.onValueChanged.AddListener(OnSliderMoved);
    }

    void OnDisable()
    {
        if (doughInputZone != null)
            doughInputZone.OnItemReceived -= SetDough;

        if (flourInputZone != null)
            flourInputZone.OnItemReceived -= AddFlour;

        if (interactionSlider != null)
            interactionSlider.onValueChanged.RemoveListener(OnSliderMoved);
    }

    void Start()
    {
        currentStickiness = initialStickiness;
        if (warningText != null) warningText.gameObject.SetActive(false);
    }

    public void SetDough(GameObject dough)
    {
        if (currentDough != null) return;

        currentDough = dough;
        currentProgress = 0f;
        currentStickiness = initialStickiness;
        
        // 重置进度条 UI
        if (ProgressUIManager.Instance != null)
        {
            ProgressUIManager.Instance.UpdateProgress(0, requiredProgress);
        }

        Debug.Log("面团已就位，请左右滑动 Slider 加工");
    }

    // ⭐ Slider 左右移动时触发
    void OnSliderMoved(float value)
    {
        if (currentDough == null) return;

        // 1. 检查粘稠度
        if (currentStickiness > stickinessThreshold)
        {
            if (warningText != null)
            {
                warningText.text = "too sticky! Add flour to continue.";
                warningText.gameObject.SetActive(true);
            }
            return; // 粘稠度太高，停止加工逻辑
        }
        else
        {
            if (warningText != null) warningText.gameObject.SetActive(false);
        }

        // 2. 计算滑动增量（只要在动就加进度）
        float delta = Mathf.Abs(value - lastSliderValue);
        if (delta > 0.01f) // 只有显著移动才算
        {
            currentProgress += progressPerMove;
            lastSliderValue = value;

            // 3. 更新进度条 UI
            if (ProgressUIManager.Instance != null)
            {
                ProgressUIManager.Instance.UpdateProgress(currentProgress, requiredProgress);
            }

            Debug.Log($"加工进度: {currentProgress}/{requiredProgress}");

            // 4. 完成检查
            if (currentProgress >= requiredProgress)
            {
                if (currentStickiness <= requiredStickinessForCompletion)
                {
                    CompleteShape();
                }
                else
                {
                    if (warningText != null)
                    {
                        warningText.text = "Last step: Stickiness needs to drop to " + requiredStickinessForCompletion;
                        warningText.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void AddFlour(GameObject flour)
    {
        if (currentDough == null)
        {
            Destroy(flour);
            return;
        }

        currentStickiness = Mathf.Max(0f, currentStickiness - stickinessDecreasePerFlour);
        Debug.Log("加入面粉，当前粘稠度: " + currentStickiness);

        // 如果粘稠度降下来了，关闭警告
        if (currentStickiness <= stickinessThreshold && warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }

        Destroy(flour);
    }

    void CompleteShape()
    {
        Debug.Log("面团造型完成！");
        if (ProgressUIManager.Instance != null) ProgressUIManager.Instance.Hide();

        GameObject newItem = Instantiate(outputPrefab, spawnPoint.position, spawnPoint.rotation);

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

    // 原有的 ProcessInput 保留，可以用来调试
    public void ProcessInput()
    {
        Debug.Log($"进度: {currentProgress}, 粘稠度: {currentStickiness}");
    }
}