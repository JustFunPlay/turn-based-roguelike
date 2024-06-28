using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sFXSlider;

    public AudioMixer musicMixer;
    public AudioMixer sFXMixer;

    public TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        float prefMusicVolume = PlayerPrefs.GetFloat("MusicPref");
        musicSlider.value = prefMusicVolume;
        musicMixer.SetFloat("Music", prefMusicVolume);

        float prefSFXVolume = PlayerPrefs.GetFloat("SFXPref");
        sFXSlider.value = prefSFXVolume;
        sFXMixer.SetFloat("SFX", prefSFXVolume);

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int i = 0;i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        IEnumerable<string> distinctOptions = options.Distinct();

        resolutionDropdown.AddOptions(distinctOptions.ToList());
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("Music", volume);
        PlayerPrefs.SetFloat("MusicPref", volume);
    }
    public void SetSFXVolume(float volume)
    {
        sFXMixer.SetFloat("SFX", volume);
        PlayerPrefs.SetFloat("SFXPref", volume);
    }
    public void SetFullscreen(bool fullscreen)
    {
        Screen.fullScreen = fullscreen;
    }
}
