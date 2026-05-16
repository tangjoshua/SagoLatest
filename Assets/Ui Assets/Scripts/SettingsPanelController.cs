using UnityEngine;

public class SettingsPanelController : MonoBehaviour
{
    public GameObject settingsPanel;

    // 打开
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // 关闭
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}