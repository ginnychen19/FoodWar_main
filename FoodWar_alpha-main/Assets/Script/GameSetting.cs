using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSetting : MonoBehaviour
{
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider MouseSlider;

    private void OnEnable()
    {
        MusicSlider.value = SaveManager.instance.nowData.musicVolume;
        SFXSlider.value = SaveManager.instance.nowData.SFXVolume;
        MouseSlider.value = SaveManager.instance.nowData.mouseSensitive;


        MouseSlider.onValueChanged.RemoveAllListeners();
        MouseSlider.onValueChanged.AddListener(delegate { ChangeMouseSensitive(); });

        MusicSlider.onValueChanged.RemoveAllListeners();
        MusicSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(); });


        SFXSlider.onValueChanged.RemoveAllListeners();
        SFXSlider.onValueChanged.AddListener(delegate { ChangeSFXVolume(); });
    }
    private void ChangeMouseSensitive()
    {
        SaveManager.instance.nowData.mouseSensitive = MouseSlider.value;
        SaveManager.instance.SaveGame();
    }

    private void ChangeMusicVolume()
    {
        SaveManager.instance.nowData.musicVolume = MusicSlider.value;
        SaveManager.instance.SaveGame();
        MusicManager.instance.musicM.SetFloat("BGM", MusicSlider.value);
    }

    private void ChangeSFXVolume()
    {
        SaveManager.instance.nowData.SFXVolume = SFXSlider.value;
        SaveManager.instance.SaveGame();
        MusicManager.instance.musicM.SetFloat("SFX", SFXSlider.value);
    }
}
