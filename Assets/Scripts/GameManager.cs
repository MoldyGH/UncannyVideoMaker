using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    string pathToAssets;
    int uncannyIndex = 0;

    public TMP_Text titleText, subtitleText;

    public Image mrIncredible, uncannyPhoto;

    string[] sequenceTimes, subtitles, titles;

    public AudioSource audioPlayer;
    public AudioClip[] clips;
    void Start()
    {
        Cursor.visible = false;
        pathToAssets = Path.Combine(Application.streamingAssetsPath, PlayerPrefs.GetString("CurrentVideo"));

        SetTitle();
        GetSubtitles();
        GetSequenceTimes();

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
    IEnumerator StartNewPhase()
    {
        mrIncredible.sprite = LoadImage(Path.Combine(pathToAssets, "phases", $"{uncannyIndex}.png"));
        uncannyPhoto.sprite = LoadImage(Path.Combine(pathToAssets, "images", $"{uncannyIndex}.png"));
        subtitleText.text = subtitles[uncannyIndex];
        audioPlayer.clip = LoadClip(Path.Combine(pathToAssets, "music", $"{uncannyIndex}.wav"));
        audioPlayer.Play();
        yield return new WaitForSeconds(float.Parse(sequenceTimes[uncannyIndex]));
        uncannyIndex++;
        if(uncannyIndex < sequenceTimes.Length)
        {
            StartCoroutine(StartNewPhase());
        }
        else
        {
            SceneManager.LoadScene("Title");
        }
    }
}
