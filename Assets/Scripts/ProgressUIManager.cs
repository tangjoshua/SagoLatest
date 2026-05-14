using UnityEngine;
using UnityEngine.UI;

public class ProgressUIManager : MonoBehaviour
{
    public static ProgressUIManager Instance;

    public Slider progressSlider;

    void Awake()
    {
        Instance = this;

        Hide();
    }

    public void Show()
    {
        if (progressSlider != null)
        {
            progressSlider.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (progressSlider != null)
        {
            progressSlider.gameObject.SetActive(false);
        }
    }

    public void UpdateProgress(float current, float max)
    {
        if (progressSlider == null) return;

        Show();

        progressSlider.value =
            current / max;
    }
}