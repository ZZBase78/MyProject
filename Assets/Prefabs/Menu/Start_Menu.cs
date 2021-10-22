using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Start_Menu : MonoBehaviour
{

    public GameObject slider_mouse_speed_go;
    public GameObject dropdown_quality;
    public GameObject fullscreen;
    public GameObject mainpanel;

    public GameObject[] secondarypanels;

    public GameObject loading_panel;
    public Image loading_image;
    public Text loading_text;

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

    //public void SetSliderValue(float value)
    //{
    //    Slider slider = slider_go.GetComponent<Slider>();
    //    slider.value = value;
    //}
    //public float GetSliderValue()
    //{
    //    Slider slider = slider_go.GetComponent<Slider>();
    //    return slider.value;
    //}

    //public void Volume_Slider_Value_Changed()
    //{
    //    AudioListener.volume = GetSliderValue();
    //    audioMixer.SetFloat("Master", GetSliderValue() * 100 - 80);
    //}

    public void SetQualityLevel()
    {
        QualitySettings.SetQualityLevel(dropdown_quality.GetComponent<Dropdown>().value);
    }

    private void Awake()
    {
        loading_panel.SetActive(false);

        //audioMixer.SetFloat("Master", 0f);
        //audioMixer.SetFloat("Sound", 0f);
        //audioMixer.SetFloat("Effects", 0f);

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

    public void Click_Start()
    {
        //Global.SetPauseGame(false);
        //SceneManager.LoadScene(1);
        StartCoroutine("LoadLevel");
        
    }

    IEnumerator LoadLevel()
    {
        yield return null;

        loading_panel.SetActive(true);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);

        while (!asyncOperation.isDone)
        {
            loading_text.text = "" + (asyncOperation.progress * 100) + "%";
            yield return null;
        }
    }

    public void Click_Quit()
    {
        Application.Quit();
    }
}
