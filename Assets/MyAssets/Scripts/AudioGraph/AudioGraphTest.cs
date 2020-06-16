using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AudioGraphTest : MonoBehaviour
{
    [SerializeField] private AudioGraph audioGraph;
    [SerializeField] private AudioPlayer audioPlayer;


    private void Awake()
    {
        audioPlayer.onPlayNextClip += QueueNextClip;
    }

    private void Start()
    {
        QueueNextClip();
    }

    [Button("Test")]
    public void QueueNextClip()
    {
        audioPlayer.QueueClip(audioGraph.GetNextClip());
    }
}
