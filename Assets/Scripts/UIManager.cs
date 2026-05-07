using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField] private GameObject settingsPanel; // 在编辑器里拖入面板

    private void Awake() {
        Instance = this;
        if(settingsPanel != null) settingsPanel.SetActive(false);
    }

    // --- 核心过程：打开设置 ---
    public void OpenSettings() {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f; // 暂停游戏逻辑和物理
        Debug.Log("游戏暂停，UI打开");
    }

    // --- 核心过程：关闭设置 ---
    public void CloseSettings() {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f; // 恢复正常速度
        Debug.Log("游戏恢复，UI关闭");
    }
}