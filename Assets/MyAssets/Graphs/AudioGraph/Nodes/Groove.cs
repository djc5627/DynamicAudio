using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using XNode;
using System.Linq;
using System.Runtime.CompilerServices;
using TreeEditor;
using Random = UnityEngine.Random;

public class Groove : Node
{
	public string GrooveName;
	[Range(1, 3)] public int intensity = 1;
	[Header("---------------------------------------------------------------------------")]
	public int startPlayCount;
	public AudioClip[] startClips;
	[Header("---------------------------------------------------------------------------")]
	public int middlePlayCount;
	public AudioClip[] middleClips;
	[Header("---------------------------------------------------------------------------")]
	public int endPlayCount;
	public AudioClip[] endClips;
	[Header("---------------------------------------------------------------------------")]
	public Groove[] nextGrooves;
	
	public AudioClip currentClip { get; private set; }
	public AudioClip QueuedClip { get; private set; }

	private List<AudioClip> startClipsPlayed = new List<AudioClip>();
	private List<AudioClip> middleClipsPlayed = new List<AudioClip>();
	private List<AudioClip> endClipsPlayed = new List<AudioClip>();
	private int currentStartCount = 0;
	private int currentMiddleCount = 0;
	private int currentEndCount = 0;

	public delegate void OnFinishGroove(Groove nextGroove);

	public event OnFinishGroove onFinishGroove;

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

		currentClip = null;
		QueuedClip = null;
		name = GrooveName;
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

		//If this is last clip
		if (currentStartCount == startPlayCount &&
		    currentMiddleCount == middlePlayCount &&
		    currentEndCount == endPlayCount)
		{
			GrooveFinished();
		}

		//If first clip of groove
		//Set playing and Queued groove for color change in Editor
		if (QueuedClip == null)
		{
			QueuedClip = nextClip;
		}
		else
		{
			currentClip = QueuedClip;
			QueuedClip = nextClip;
		}
		
		Debug.LogError($"Current: {currentClip}");
		Debug.LogError($"Queued: {QueuedClip}");
		
		
		return nextClip;
	}

	//Reset values here
	private void GrooveFinished()
	{
		if (onFinishGroove != null)
		{
			onFinishGroove.Invoke(PickRandomGroove(nextGrooves));
			Init();
		}
	}

	private void OnValidate()
	{
		//Dont allow play count > clips[] length
		if (startPlayCount > startClips.Length)
		{
			startPlayCount = startClips.Length;
		}
		if (middlePlayCount > middleClips.Length)
		{
			middlePlayCount = middleClips.Length;
		}
		if (endPlayCount > endClips.Length)
		{
			endPlayCount = endClips.Length;
		}
	}

	private AudioClip PickRandomClip(AudioClip[] clipArray)
	{
		int randomIndex = Random.Range(0, clipArray.Length);
		return clipArray[randomIndex];
	}
	
	private Groove PickRandomGroove(Groove[] grooveArray)
	{
		int randomIndex = Random.Range(0, grooveArray.Length);
		return grooveArray[randomIndex];
	}
}