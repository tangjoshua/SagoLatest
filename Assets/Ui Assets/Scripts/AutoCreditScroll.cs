using UnityEngine;
using UnityEngine.UI;

public class AutoCreditScroll : MonoBehaviour
{
    public ScrollRect scrollRect;

    public float autoSpeed = 15f;
    public float resumeDelay = 2f;

    private float timer;

    void Update()
    {
        // ✔ 用户正在操作（拖动/滚动/惯性）
        if (scrollRect.velocity.sqrMagnitude > 5f)
        {
            timer = resumeDelay;
            return;
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        // ✔ 自动滚动
        scrollRect.verticalNormalizedPosition -= autoSpeed * Time.deltaTime * 0.0005f;

        // ❗关键修复：不要直接跳1
        if (scrollRect.verticalNormalizedPosition <= 0f)
        {
            scrollRect.StopMovement();
            scrollRect.verticalNormalizedPosition = 1f;
            scrollRect.velocity = Vector2.zero;
        }
    }
}