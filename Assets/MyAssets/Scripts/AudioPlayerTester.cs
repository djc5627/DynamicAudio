using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioPlayerTester : MonoBehaviour
{
    public AudioPlayer audioPlayer;
    public AudioClip[] clipArray;
    
    private int nextClipIndex = 0;
    private bool manualQueue = false;

    private void Awake()
    {
        audioPlayer.onPlayNextClip += SendNextClip;
    }

    private void Start()
    {
        audioPlayer.QueueClip(clipArray[nextClipIndex]);
    }

    private void Update()
    {
        CheckForReloadScene();
    }
    
    private void SendNextClip()
    {
        if (!manualQueue)
        {
            // Increase the clip index number, wrap around if it runs out of clips
            nextClipIndex = nextClipIndex < clipArray.Length - 1 ? nextClipIndex + 1 : 0;
        }
        else
        {
            manualQueue = false;
        }

        audioPlayer.QueueClip(clipArray[nextClipIndex]);
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
                manualQueue = true;
                SendNextClip();
            };
        }

        GUI.color = Color.cyan;
        GUI.TextField(new Rect(10, 10 + clipArray.Length * vertSpacing, 250, 20), $"Next Clip: {clipArray[nextClipIndex].name}");
    }
    
    private void CheckForReloadScene()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}


