using TMPro;
using UnityEngine;

public class DayUI : MonoBehaviour
{
    public static DayUI Instance;

    public TextMeshProUGUI dayText;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    public void UpdateDay(int day)
    {
        dayText.text = "Day " + day;
    }
}