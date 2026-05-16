using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    public BGMController bgmController;

    void Start()
    {
        // 先读取保存值
        bgmSlider.value =
            PlayerPrefs.GetFloat("BGMVolume", 1f);

        sfxSlider.value =
            PlayerPrefs.GetFloat("SFXVolume", 1f);

        // 再绑定事件
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);

        SFXManager.UpdateAllSFX(volume);
    }
}