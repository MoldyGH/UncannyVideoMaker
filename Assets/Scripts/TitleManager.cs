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
    public TMP_Text listThing;
    public TMP_InputField input;

    public Slider volumeSlider;
    public Toggle randomizeOrder;
    public AudioMixer mixer;
    void Start()
    {
        Cursor.visible = true;
        listThing.text = string.Empty;
        mixer.SetFloat("volume", PlayerPrefs.GetFloat("Volume", 0));
        randomizeOrder.isOn = PlayerPrefs.GetInt("RandomizeOrder", 0) == 1;

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 0);

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.streamingAssetsPath);
        DirectoryInfo[] info = directoryInfo.GetDirectories();

        foreach(DirectoryInfo f in info)
        {
            listThing.text += $"{f.Name}\n";
        }       
    }
    public void LoadVideo()
    {
        if (!string.IsNullOrEmpty(input.text) && Directory.Exists(Path.Combine(Application.streamingAssetsPath, input.text)))
        {
            PlayerPrefs.SetString("CurrentVideo", input.text);
            SceneManager.LoadScene("Main");
        }
    }
    public void ChangeVolume()
    {
        mixer.SetFloat("volume", volumeSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }

    public void ChangeOrderRandom()
    {
        PlayerPrefs.SetInt("RandomizeOrder", randomizeOrder.isOn == true ? 1 : 0);
    }
}
