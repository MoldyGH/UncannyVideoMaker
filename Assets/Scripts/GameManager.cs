using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    string pathToAssets;
    int uncannyIndex = 0;

    public TMP_Text titleText, subtitleText;

    public Image mrIncredible, uncannyPhoto;

    bool randomizeImages, clipBasedLength;

    public string[] Images = new string[1];

    string[] sequenceTimes, subtitles, titles, extraSettings;

    public AudioSource audioPlayer;
    public AudioClip[] clips;

    int numberOfPhases;
    void Start()
    {
        Cursor.visible = false;
        pathToAssets = Path.Combine(Application.streamingAssetsPath, PlayerPrefs.GetString("CurrentVideo"));
        numberOfPhases = Directory.GetFiles(Path.Combine(pathToAssets, "phases"), "*.png").Length - 1;

        SetTitle();
        GetSubtitles();
        GetSequenceTimes();
        GetExtraSettings();

        if (randomizeImages)
        {
            Images = Directory.GetFiles(Path.Combine(pathToAssets, "images"),"*.png");
            Images = Images.OrderBy(x => UnityEngine.Random.Range(-1f,1f)).ToArray();
        }

        StartCoroutine(StartNewPhase());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }
    public Sprite LoadImage(string path)
    {
        Texture2D tex = null;
        byte[] fileData;

        if(File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
    }
    public AudioClip LoadClip(string path)
    {
        AudioClip clip = null;
        byte[] fileData;

        if(File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            clip = OpenWavParser.ByteArrayToAudioClip(fileData);
        }
        return clip;
    }
    public void SetTitle()
    {
        titles = File.ReadAllLines(Path.Combine(pathToAssets, "text", "title.txt"));
        titleText.text = titles[UnityEngine.Random.Range(0, titles.Length)];
    }
    public void GetSubtitles()
    {
        subtitles = File.ReadAllLines(Path.Combine(pathToAssets, "text", "sequencetext.txt"));
    }
    public void GetSequenceTimes()
    {
        sequenceTimes = File.ReadAllLines(Path.Combine(pathToAssets, "text", "timebetweenphases.txt"));
    }
    public void GetExtraSettings()
    {
        extraSettings = File.ReadAllLines(Path.Combine(pathToAssets, "text", "extrasettings.txt"));
        foreach(string setting in extraSettings)
        {
            string[] currentSetting = setting.Trim().Split('=');
            
            if(currentSetting[0] == "randomize_images")
            {
                randomizeImages = currentSetting[1] == "true";
            }
            else if(currentSetting[0] == "length_based_on_clip")
            {
                clipBasedLength = currentSetting[1] == "true";
            }
        }
    }
    public float SetSequenceTimes()
    {
        float time = 0;
        if (clipBasedLength)
        {
            time = audioPlayer.clip.length;
        }
        else
        {
            time = float.Parse(sequenceTimes[uncannyIndex]);
        }
        return time;
    }
    IEnumerator StartNewPhase()
    {
        mrIncredible.sprite = LoadImage(Path.Combine(pathToAssets, "phases", $"{uncannyIndex}.png"));
        if (randomizeImages)
        {
            uncannyPhoto.sprite = LoadImage(Images[uncannyIndex]);
            subtitleText.text = Path.GetFileNameWithoutExtension(Images[uncannyIndex]);
        }
        else
        {
            uncannyPhoto.sprite = LoadImage(Path.Combine(pathToAssets, "images", $"{uncannyIndex}.png"));
            subtitleText.text = subtitles[uncannyIndex];
        }
        audioPlayer.clip = LoadClip(Path.Combine(pathToAssets, "music", $"{uncannyIndex}.wav"));
        audioPlayer.Play();
        yield return new WaitForSeconds(SetSequenceTimes());
        uncannyIndex++;
        if (uncannyIndex <= numberOfPhases)
        {
            StartCoroutine(StartNewPhase());
        }
        else
        {
            SceneManager.LoadScene("Title");
        }
    }
}
