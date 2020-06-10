using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using XNode;

public class AudioClipNode : Node
{
	[Input] public AudioClipNode previous;
	[Output] public AudioClipNode next;
	public AudioClip clip;
	public int startClipCount;
	public AudioClip[] startClips;
	public int middleClipCount;
	public AudioClip[] middleClips;
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
		Debug.Log($"returned: {this}");
		return this;
	}

	public void Play()
	{
		if (clip != null)
		{
			var go = new GameObject("PLAY_AUDIO_TEMP");
			AudioSource audioSource = go.AddComponent<AudioSource>();
			audioSource.clip = clip;
			audioSource.Play();
			Debug.Log("Play");
		}
	}
}