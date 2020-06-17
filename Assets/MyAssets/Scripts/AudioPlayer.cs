using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource[] AudioSources;
    
    public Slider audioSlider;
    public TMP_Text playingText;
    public float initialDelay = 1f;
    public float scheduleAheadTime = 1f;

    private AudioClip nextClip;
    private int toggle = 0;
    private double currentClipStartTime;
    private double nextStartTime;
    private double currentClipDuration;
    private double nextClipDuration;
    private double currentStartTime;
    private bool isFirstClip = true;

    public delegate void OnPlayNextClip();

    public event OnPlayNextClip onPlayNextClip;
    
    private void Update()
    {
        HandleSwitchingClips();
        UpdateSlider();
    }

    public void QueueClip(AudioClip clip)
    {
        nextClip = clip;
        
        //If first clip, do initial delay (assumes its fed clips nonstop)
        if (isFirstClip)
        {
            nextStartTime = AudioSettings.dspTime + initialDelay;
            isFirstClip = false;
        }
        
        Debug.Log($"Queued Clip: {nextClip}");
    }

    private void HandleSwitchingClips()
    {
        //if 1 second before clip ends
        if (AudioSettings.dspTime > nextStartTime - scheduleAheadTime)
        {
            // Loads the next Clip to play and schedules when it will start
            AudioSources[toggle].clip = nextClip;
            AudioSources[toggle].PlayScheduled(nextStartTime);
            
            // Switches the toggle to use the other Audio Source next
            toggle = 1 - toggle;
            
            // Checks how long the Clip will last and updates the Next Start Time with a new value
            nextClipDuration = (double) nextClip.samples / nextClip.frequency;
            currentStartTime = nextStartTime;
            nextStartTime += nextClipDuration;

            Debug.Log($"Loaded Clip: {nextClip}");
            
            //Update start time/duration for slider (after already playing)
            LeanTween.value(0f, 1f, scheduleAheadTime)
                .setOnComplete(_ =>
                {
                    currentClipStartTime = currentStartTime;
                    currentClipDuration = nextClipDuration;
                    
                    Debug.Log($"Playing Clip: {nextClip}");
                    if (playingText != null) playingText.text = $"Playing Clip:\n{nextClip.name}";
                    
                    //Let others know clip is switched and ready for Queue
                    if (onPlayNextClip != null) onPlayNextClip.Invoke();
                });
        }
    }

    private void UpdateSlider()
    {
        if (audioSlider == null) return;
        audioSlider.value = (float) ((AudioSettings.dspTime - currentClipStartTime) / currentClipDuration);
        // Debug.Log($"-----------START------------");
        // Debug.Log($"SliderVal: {audioSlider.value}");
        // Debug.Log($"CurrTime: {AudioSettings.dspTime}");
        // Debug.Log($"StartTime: {currentClipStartTime}");
        // Debug.Log($"Duration: {currentClipDuration}");
    }

    
}
