using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [Header("Sound Slider")]
    public Slider allMusicSetting;
    public Slider backgroundMusicSetting;
    public Slider soundEffectSetting;

    public bool backgroundMusic = false;

    AudioSource audioS;

    public static float backgroundVolume = 0.5f;
    public static float effectVolume = 0.5f;

    private void Awake()
    {
        if(allMusicSetting)
            allMusicSetting.value = SoundSetting.backgroundVolume;

        if (soundEffectSetting)
            soundEffectSetting.value = SoundSetting.effectVolume;
    }

    private void Start()
    {
        audioS = GetComponent<AudioSource>();
        if (backgroundMusic) audioS.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        if (audioS)
        {
            if (backgroundMusic)
            {
                audioS.volume = backgroundVolume;
            }
            else
            {
                audioS.volume = effectVolume;
            }
        }
    }

    public void BackgroundMusicSetting(float value)
    {
        backgroundVolume = value;
    }

    public void EffectMusicSetting(float value)
    {
        effectVolume = value;
    }
}
