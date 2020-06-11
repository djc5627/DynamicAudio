using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using XNode;

public class Groove : Node
{
	public string Name;
	[Range(1, 3)] public int intensity = 1;
	[Header("Start Clips-------------")]
	public int startClipCount;
	public AudioClip[] startClips;
	[Header("Middle Clips------------")]
	public int middleClipCount;
	public AudioClip[] middleClips;
	[Header("End Clips---------------")]
	public int endClipCount;
	public AudioClip[] endClips;
	public AudioClip[] nextGrooves;

	// Use this for initialization
	protected override void Init() {
		base.Init();
	}
	

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null;
	}

	public void Play()
	{
		// if (clip != null)
		// {
		// 	var go = new GameObject("PLAY_AUDIO_TEMP");
		// 	AudioSource audioSource = go.AddComponent<AudioSource>();
		// 	audioSource.clip = clip;
		// 	audioSource.Play();
		// 	Debug.Log("Play");
		// }
	}
}