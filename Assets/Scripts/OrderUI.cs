using UnityEngine;
using TMPro;

public class OrderUI : MonoBehaviour
{
    public static OrderUI Instance;

    public GameObject panel;
    public TextMeshProUGUI text;

    void Awake()
    {
        Instance = this;
    }

    public void ShowOrder(OrderData order)
    {
        panel.SetActive(true);

        text.text =
            $"需求:\n" +
            $"- Tag: {order.requiredTag}\n" +
            $"- CookTime: {order.minCookTime}\n" +
            $"- Refine: {order.minRefine}\n\n" +
            $"故事:\n{order.dialogue}";
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
