using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using XNode;

public class Groove : Node
{
	public string Name;
	[Range(1, 3)] public int intensity = 1;
	[Header("-----Start Clips--------------------------")]
	public int startPlayCount;
	public AudioClip[] startClips;
	[Header("-----Middle Clips-------------------------")]
	public int middlePlayCount;
	public AudioClip[] middleClips;
	[Header("-----End Clips----------------------------")]
	public int endPlayCount;
	public AudioClip[] endClips;
	public AudioClip[] nextGrooves;

	private List<AudioClip> startClipsPlayed = new List<AudioClip>();
	private List<AudioClip> middleClipsPlayed = new List<AudioClip>();
	private List<AudioClip> endClipsPlayed = new List<AudioClip>();

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
		startClipsPlayed.Clear();
		middleClipsPlayed.Clear();
		endClipsPlayed.Clear();
	}
	

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null;
	}

	public void StartGroove()
	{
		Debug.Log($"Started playing groove: {name}");
		
	}
}