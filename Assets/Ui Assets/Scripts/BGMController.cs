using UnityEngine;

public class BGMController : MonoBehaviour
{
    public static AudioSource currentBGM;

    void Awake()
    {
        currentBGM = GetComponent<AudioSource>();

        currentBGM.volume =
            PlayerPrefs.GetFloat("BGMVolume", 1f);
    }
}