using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{

    public Slider volumeSlider;
    public AudioMixer mixer;

    public TMP_Dropdown dropdown;
    void Start()
    {
        Cursor.visible = true;
        mixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume", 0));
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0);

        SetUpDropdown();

        dropdown.value = PlayerPrefs.GetInt("SelectedDropdown", 0);
        dropdown.RefreshShownValue();
    }
    public void SetUpDropdown()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        DirectoryInfo[] info = directoryInfo.GetDirectories();
        dropdown.ClearOptions();
        List<string> list = new List<string>();

        foreach(DirectoryInfo f in info)
        {
            list.Add(f.Name);
        }
        list.Sort();
        dropdown.AddOptions(list);
    }
    public void LoadVideo()
    {
        if (Directory.Exists(Path.Combine(Application.streamingAssetsPath, dropdown.options[dropdown.value].text)))
        {
            PlayerPrefs.SetString("CurrentVideo", dropdown.options[dropdown.value].text);
            PlayerPrefs.SetInt("SelectedDropdown", dropdown.value);
            SceneManager.LoadScene("Main");
        }
    }
    public void ChangeVolume()
    {
        mixer.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
