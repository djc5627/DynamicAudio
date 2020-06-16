using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using XNode;
using System.Linq;
using Random = UnityEngine.Random;

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
	private int currentStartCount = 0;
	private int currentMiddleCount = 0;
	private int currentEndCount = 0;

	// Use this for initialization
	protected override void Init() {
		base.Init();
		
		startClipsPlayed.Clear();
		middleClipsPlayed.Clear();
		endClipsPlayed.Clear();

		//Random sort clips
		startClips = startClips.OrderBy(_ => Guid.NewGuid()).ToArray();
		middleClips = middleClips.OrderBy(_ => Guid.NewGuid()).ToArray();
		endClips = endClips.OrderBy(_ => Guid.NewGuid()).ToArray();

		currentStartCount = 0;
		currentMiddleCount = 0;
		currentEndCount = 0;
	}
	

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port)
	{
		return null;
	}

	public AudioClip GetNextClip()
	{
		AudioClip nextClip = null;
		if (currentStartCount < startPlayCount)
		{
			nextClip = startClips[currentStartCount];
			currentStartCount++;
		}
		else if (currentMiddleCount < middlePlayCount)
		{
			nextClip = middleClips[currentMiddleCount];
			currentMiddleCount++;
		}
		else if (currentEndCount < endPlayCount)
		{
			nextClip = endClips[currentEndCount];
			currentEndCount++;
		}

		return nextClip;
	}

	private AudioClip PickRandomClip(AudioClip[] clipArray)
	{
		int randomIndex = Random.Range(0, clipArray.Length);
		return clipArray[randomIndex];
	}
}