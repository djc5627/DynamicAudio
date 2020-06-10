using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource[] AudioSources;
    public AudioClip[] clipArray;
    public Slider audioSlider;
    public float initialDelay = 1f;
    public float scheduleAheadTime = 1f;

    private int toggle = 0;
    private int nextClipIndex = 0;
    private double currentClipStartTime;
    private double currentClipDuration;
    private double nextStartTime;
    private double currentStartTime;


    private void Awake()
    {
        nextStartTime = AudioSettings.dspTime + initialDelay;
        currentClipStartTime = nextStartTime;
        LeanTween.value(0f, 1f, initialDelay)
            .setOnComplete(_ =>
            {
                currentClipStartTime = nextStartTime;
            });
    }

    private void Update()
    {
        CheckForReloadScene();
        HandleSwitchingClips();
        UpdateSlider();
    }

    private void HandleSwitchingClips()
    {
        //if 1 second before clip ends
        if (AudioSettings.dspTime > nextStartTime - scheduleAheadTime)
        {
            AudioClip clipToPlay = clipArray[nextClipIndex];

            // Loads the next Clip to play and schedules when it will start
            AudioSources[toggle].clip = clipToPlay;
            AudioSources[toggle].PlayScheduled(nextStartTime);

            // Checks how long the Clip will last and updates the Next Start Time with a new value
            double duration = (double)clipToPlay.samples / clipToPlay.frequency;
            currentStartTime = nextStartTime;
            nextStartTime += duration;

            // Switches the toggle to use the other Audio Source next
            toggle = 1 - toggle;

            // Increase the clip index number, reset if it runs out of clips
            nextClipIndex = nextClipIndex < clipArray.Length - 1 ? nextClipIndex + 1 : 0;
            
            //Update start time/duration for slider
            LeanTween.value(0f, 1f, scheduleAheadTime)
                .setOnComplete(_ =>
                {
                    currentClipStartTime = currentStartTime;
                    currentClipDuration = duration;
                });
        }
    }

    private void CheckForReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateSlider()
    {
        audioSlider.value = (float) ((AudioSettings.dspTime - currentClipStartTime) / currentClipDuration);
        // Debug.Log($"-----------START------------");
        // Debug.Log($"SliderVal: {audioSlider.value}");
        // Debug.Log($"CurrTime: {AudioSettings.dspTime}");
        // Debug.Log($"StartTime: {currentClipStartTime}");
        // Debug.Log($"Duration: {currentClipDuration}");
    }

    private void OnGUI()
    {
        float vertSpacing = 30f;
        float horrSpacing = 30f;
        for (int i = 0; i < clipArray.Length; i++)
        {
            GUI.color = (i == nextClipIndex) ? Color.green : Color.white;
            GUI.TextField(new Rect(10, 10 + i*vertSpacing, 200, 20),
                clipArray[i].name);
            if (GUI.Button(new Rect(210 + horrSpacing, 10 + i * vertSpacing, 50, 20), "+"))
            {
                nextClipIndex = i;
            };
        }

        GUI.color = Color.cyan;
        GUI.TextField(new Rect(10, 10 + clipArray.Length * vertSpacing, 250, 20), $"Next Clip: {clipArray[nextClipIndex].name}");
    }
}
