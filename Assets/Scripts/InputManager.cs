using UnityEngine;

public class InputManager : MonoBehaviour
{
    private float holdTime = 0.5f;
    private float timer;
    private bool isHolding;

    private DragItem currentDragItem;
    public static bool IsUsingInput = false;

    void Update()
    {
        HandleMouse();
    }

    void HandleMouse()
    {
        if (Camera.main == null) return;

        // =========================
        // 🟢 鼠标按下
        // =========================
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                timer = 0f;
                isHolding = true;

                // ⭐ 1️⃣ NPC优先（防止拖拽冲突）
                NPCOrder npc = hit.collider.GetComponentInParent<NPCOrder>();
                if (npc != null)
                {
                    npc.OnClickOrder();
                    return; // ❗关键：直接结束
                }

                // ⭐ 2️⃣ 拖拽物体（OrderPaper / Food / Ingredient）
                DragItem drag = hit.collider.GetComponentInParent<DragItem>();
                if (drag != null)
                {
                    IsUsingInput = true;
                    currentDragItem = drag;
                    currentDragItem.StartDrag();
                }

                // ⭐ 3️⃣ Station 点击
                ShapeStation shape = hit.collider.GetComponentInParent<ShapeStation>();
                if (shape != null)
                {
                    shape.ProcessInput();
                }
            }
        }

        // =========================
        // 🟢 长按检测
        // =========================
        if (Input.GetMouseButton(0) && isHolding)
        {
            timer += Time.deltaTime;

            if (timer >= holdTime)
            {
                isHolding = false;
                HandleHold(Input.mousePosition);
            }
        }

        // =========================
        // 🟢 松开
        // =========================
        if (Input.GetMouseButtonUp(0))
        {
            if (currentDragItem != null)
            {
                currentDragItem.StopDrag();
                currentDragItem = null;
            }

            if (timer < holdTime)
            {
                HandleTap(Input.mousePosition);
            }

            IsUsingInput = false;
            isHolding = false;
        }
    }

    // =========================
    // 🟢 点击（短按）
    // =========================
    void HandleTap(Vector2 pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                Debug.Log("Tap: " + hit.collider.name);
            }
        }
    }

    // =========================
    // 🟢 长按（Cooking）
    // =========================
    void HandleHold(Vector2 pos)
    {
        if (Camera.main == null) return;

        Ray ray = Camera.main.ScreenPointToRay(pos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hold: " + hit.collider.name);

            CookingStation cook = hit.collider.GetComponentInParent<CookingStation>();

            if (cook != null)
            {
                cook.StartCooking();
            }
        }
    }
}
