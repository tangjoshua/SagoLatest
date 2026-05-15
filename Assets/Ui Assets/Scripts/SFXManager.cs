using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static List<AudioSource> allSFX =
        new List<AudioSource>();

    public static void RegisterSFX(AudioSource source)
    {
        if (!allSFX.Contains(source))
        {
            allSFX.Add(source);

            source.volume =
                PlayerPrefs.GetFloat("SFXVolume", 1f);
        }
    }

    public static void UpdateAllSFX(float volume)
    {
        foreach (AudioSource sfx in allSFX)
        {
            if (sfx != null)
            {
                sfx.volume = volume;
            }
        }
    }
}