using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class OptionMenu : MonoBehaviour
{
    public AudioMixer mixerToken;
    public TMP_Dropdown dropDownToken;
    public Toggle toggleToken;
    [SerializeField] Resolution[] allResolution;
    [SerializeField] List<Resolution> supportResolution = new();
    [SerializeField] List<string> stringResolution = new();

    private void Start()
    {
        //Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, Screen.fullScreen);
        Debug.Log(Screen.currentResolution);
        allResolution = Screen.resolutions;
        dropDownToken.ClearOptions();
        toggleToken.isOn = Screen.fullScreen;
        int currentResolutionIndex = 0;
        for (int i = 0; i < allResolution.Length; i++)
        {
            if (59 < allResolution[i].refreshRateRatio.value && allResolution[i].refreshRateRatio.value <= 60.1f)
            {
                string option = allResolution[i].width + " x " + allResolution[i].height + " " + allResolution[i].refreshRateRatio + "Hz";
                stringResolution.Add(option);
                supportResolution.Add(allResolution[i]);
                if (allResolution[i].width == Screen.width
                    && allResolution[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
        }
        dropDownToken.AddOptions(stringResolution);
        dropDownToken.value = currentResolutionIndex;
        dropDownToken.RefreshShownValue();
    }
    
    public void DropDownClick()
    {
        Cursor.visible = true;
    }
    public void DropDownExit()
    {
        Cursor.visible = false;
    }

    public void SetMasterVol(float value) 
    {
        int temp;
        if (value == 0) temp = -80;
        else temp = -40 + (int)(4 * value);
        mixerToken.SetFloat("masterVol", temp);
    }
    public void SetMusicVol(float value)
    {
        int temp;
        if (value == 0) temp = -80;
        else temp = -40 + (int)(4 * value);
        mixerToken.SetFloat("musicVol", temp);
    }
    public void SetSFXVol(float value)
    {
        int temp;
        if (value == 0) temp = -80;
        else temp = -40 + (int)(4 * value);
        mixerToken.SetFloat("sfxVol", temp);
    }

    public void SetResolution(int index) 
    {
        Screen.SetResolution(supportResolution[index].width, supportResolution[index].height, Screen.fullScreen);
    }
    public void SetFullScreen(bool toggle) 
    {
        if (toggle) Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }

}
