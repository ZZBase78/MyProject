using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Pause : MonoBehaviour
{

    public GameObject slider_go;
    public GameObject slider_mouse_speed_go;
    public GameObject dropdown_quality;
    public GameObject fullscreen;
    public GameObject mainpanel;

    public GameObject[] secondarypanels;

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

    public void SetSliderValue(float value)
    {
        Slider slider = slider_go.GetComponent<Slider>();
        slider.value = value;
    }
    public float GetSliderValue()
    {
        Slider slider = slider_go.GetComponent<Slider>();
        return slider.value;
    }

    public void Volume_Slider_Value_Changed()
    {
        AudioListener.volume = GetSliderValue();
    }

    public void SetQualityLevel()
    {
        QualitySettings.SetQualityLevel(dropdown_quality.GetComponent<Dropdown>().value);
    }

    private void Awake()
    {
        SetSliderValue(AudioListener.volume);
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
}
