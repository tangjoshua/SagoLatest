using UnityEngine;

public class BGMController : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
    }
}