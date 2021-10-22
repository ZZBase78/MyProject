using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu_Pause : MonoBehaviour
{

    public GameObject slider_mouse_speed_go;
    public GameObject dropdown_quality;
    public GameObject fullscreen;
    public GameObject mainpanel;

    public GameObject[] secondarypanels;

    public AudioMixer audioMixer;

    public Slider slider_master_volume;
    public Slider slider_sound_volume;
    public Slider slider_effects_volume;

    public void SetVolumeFromSlider()
    {
        audioMixer.SetFloat("Master", Global.GetVolumeFromFloat(slider_master_volume.value));
        audioMixer.SetFloat("Sound", Global.GetVolumeFromFloat(slider_sound_volume.value));
        audioMixer.SetFloat("Effects", Global.GetVolumeFromFloat(slider_effects_volume.value));

    }
    public void SetSliderFromVolume()
    {
        float volume;
        audioMixer.GetFloat("Master", out volume);
        slider_master_volume.value = Global.GetFloatFromVolume(volume);
        audioMixer.GetFloat("Sound", out volume);
        slider_sound_volume.value = Global.GetFloatFromVolume(volume);
        audioMixer.GetFloat("Effects", out volume);
        slider_effects_volume.value = Global.GetFloatFromVolume(volume);
    }

    public void Click_MainMenu()
    {
        Global.SetPauseGame(false);
        Global.Cursor_On();
        SceneManager.LoadScene(0);
    }
    public void SetFullScreen()
    {
        Screen.fullScreen = fullscreen.GetComponent<Toggle>().isOn;
        //if (!Screen.fullScreen)
        //{
        //    Screen.fullScreenMode = FullScreenMode.Windowed;
        //}
        //else
        //{
        //    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        //}
    }

    public void SetSliderMouseSpeedValue(float value)
    {
        Slider slider = slider_mouse_speed_go.GetComponent<Slider>();
        slider.value = value;
    }
    public float GetSliderMouseSpeedValue()
    {
        Slider slider = slider_mouse_speed_go.GetComponent<Slider>();
        return slider.value;
    }

    public void Volume_Slider_MouseSpeed_Value_Changed()
    {
        Global.mouse_speed = GetSliderMouseSpeedValue();
    }

    public void SetQualityLevel()
    {
        QualitySettings.SetQualityLevel(dropdown_quality.GetComponent<Dropdown>().value);
    }

    private void Awake()
    {
        SetSliderFromVolume();

        SetSliderMouseSpeedValue(Global.mouse_speed);

        dropdown_quality.GetComponent<Dropdown>().value = QualitySettings.GetQualityLevel();
        fullscreen.GetComponent<Toggle>().isOn = Screen.fullScreen;

        foreach(GameObject sp in secondarypanels)
        {
            sp.SetActive(false);
        }

        mainpanel.SetActive(true);
    }

    public void Click_Continue()
    {
        Global.SetPauseGame(false);
    }

    public void Click_Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Click_Continue();
        }
    }
}
